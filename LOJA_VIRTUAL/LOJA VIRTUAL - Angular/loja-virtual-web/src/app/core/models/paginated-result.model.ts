export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
}