import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, forkJoin, of, switchMap, Observable } from 'rxjs';
import { AdminService } from '../../services/admin.service';
import { ProdutoCreateDTO, VariacaoProduto, VariacaoProdutoCreateDTO } from '../../../../core/models/produto.model';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-product-management',
  standalone: false,
  templateUrl: './product-management.component.html',
  styleUrls: ['./product-management.component.scss']
})
export class ProductManagementComponent implements OnInit {
  produtoForm!: FormGroup;
  isLoading = false;
  isEditMode = false;
  currentProductId: number | null = null;
  pageTitle = 'Criar Novo Produto';
  successMessage: string | null = null;
  errorMessage: string | null = null;
  variationUploadStates = new Map<number, boolean>();
  
  public apiUrl = environment.apiUrl.replace(/\/$/, '');
  public placeholderImg = 'https://placehold.co/150x150/f9f9f9/ddd?text=Imagem%20Inválida';

  private deletedVariationIds: number[] = [];

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.checkEditMode();
  }

  private initForm(): void {
    this.produtoForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      descricao: [''],
      categoria: [''],
      variacoes: this.fb.array([])
    });
  }

  private checkEditMode(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.currentProductId = +id;
        this.pageTitle = 'Editar Produto';
        this.loadProductData(+id);
      } else {
        this.isEditMode = false;
        this.pageTitle = 'Criar Novo Produto';
        this.adicionarVariacao();
      }
    });
  }

  private loadProductData(id: number): void {
    this.isLoading = true;
    this.adminService.getProdutoById(id).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (produto) => {
        this.produtoForm.patchValue({
          nome: produto.nome,
          descricao: produto.descricao,
          categoria: produto.categoria
        });
        this.setVariacoes(produto.variacoes);
      },
      error: () => {
        this.errorMessage = "Produto não encontrado ou erro ao carregar dados.";
        this.router.navigate(['/admin/produtos']);
      }
    });
  }

  private setVariacoes(variacoes: VariacaoProduto[]): void {
    this.variacoes.clear();
    if (variacoes && variacoes.length > 0) {
      variacoes.forEach(v => {
        this.variacoes.push(this.fb.group({
          id: [v.id],
          tamanho: [v.tamanho, Validators.required],
          cor: [v.cor],
          preco: [v.preco, [Validators.required, Validators.min(0.01)]],
          quantidadeEstoqueInicial: [v.estoque?.quantidade ?? 0, [Validators.required, Validators.min(0)]],
          imagemUrl: [v.imagemUrl || ''] 
        }));
      });
    } else {
      this.adicionarVariacao();
    }
  }

  get variacoes(): FormArray {
    return this.produtoForm.get('variacoes') as FormArray;
  }

  criarVariacaoFormGroup(): FormGroup {
    return this.fb.group({
      id: [null],
      tamanho: ['', Validators.required],
      cor: [''],
      preco: [0, [Validators.required, Validators.min(0.01)]],
      quantidadeEstoqueInicial: [0, [Validators.required, Validators.min(0)]],
      imagemUrl: ['']
    });
  }

  adicionarVariacao(): void {
    this.variacoes.push(this.criarVariacaoFormGroup());
  }

  onFileSelected(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      return;
    }

    const file = input.files[0];
    const variacaoFormGroup = this.variacoes.at(index);

    this.variationUploadStates.set(index, true);
    this.errorMessage = null;

    this.adminService.uploadImagem(file).pipe(
      finalize(() => this.variationUploadStates.set(index, false))
    ).subscribe({
      next: (response) => {
        variacaoFormGroup.patchValue({ imagemUrl: response.url });
        input.value = '';
      },
      error: (err) => {
        this.errorMessage = `Erro ao enviar imagem da Variação ${index + 1}. Tente novamente.`;
        input.value = '';
      }
    });
  }

  removerVariacao(index: number): void {
    if (this.variacoes.length <= 1 && !this.isEditMode) {
      return;
    }

    const variacaoRemovida = this.variacoes.at(index);
    const id = variacaoRemovida.get('id')?.value;

    if (id) {
      this.deletedVariationIds.push(id);
    }

    this.variacoes.removeAt(index);
  }

  onSubmit(): void {
    if (this.produtoForm.invalid) {
      this.produtoForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.successMessage = null;
    this.errorMessage = null;

    if (this.isEditMode && this.currentProductId) {
      this.handleUpdate();
    } else {
      this.handleCreate();
    }
  }

  private handleCreate(): void {
    const formValue: ProdutoCreateDTO = this.produtoForm.value;
    this.adminService.createProduto(formValue).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: () => {
        this.successMessage = 'Produto criado com sucesso!';
        setTimeout(() => this.router.navigate(['/admin/produtos']), 2000);
      },
      error: () => this.errorMessage = 'Erro ao criar o produto.'
    });
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = this.placeholderImg;
  }

  private handleUpdate(): void {
    const formValue = this.produtoForm.value;

    const produtoData = {
      nome: formValue.nome,
      descricao: formValue.descricao,
      categoria: formValue.categoria
    };

    this.adminService.updateProduto(this.currentProductId!, produtoData).pipe(
      switchMap(() => {
        const apiCalls: Observable<any>[] = [];

        formValue.variacoes.forEach((variacao: any) => {
          const dto: VariacaoProdutoCreateDTO = {
            tamanho: variacao.tamanho,
            cor: variacao.cor,
            preco: variacao.preco,
            quantidadeEstoqueInicial: variacao.quantidadeEstoqueInicial,
            produtoId: this.currentProductId!,
            imagemUrl: variacao.imagemUrl
          };

          if (variacao.id) {
            apiCalls.push(this.adminService.updateVariacaoProduto(variacao.id, dto));
          } else {
            apiCalls.push(this.adminService.createVariacaoProduto(dto));
          }
        });

        this.deletedVariationIds.forEach(id => {
          apiCalls.push(this.adminService.deleteVariacaoProduto(id));
        });

        return apiCalls.length > 0 ? forkJoin(apiCalls) : of(null);
      }),
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: () => {
        this.successMessage = 'Produto atualizado com sucesso!';
        this.deletedVariationIds = [];
        setTimeout(() => this.router.navigate(['/admin/produtos']), 2000);
      },
      error: () => this.errorMessage = 'Erro ao atualizar o produto ou suas variações.'
    });
  }
}