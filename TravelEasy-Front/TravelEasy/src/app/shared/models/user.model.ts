export interface User {
  id: number;
  nome: string;
  cognome: string;
  email: string;
  role: UserRole;
}

export enum UserRole {
  Customer,
  Admin,
  ProductManager,
  OrderManager,
  Writer
}
