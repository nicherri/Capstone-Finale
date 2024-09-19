import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../shared/models/product.model';
import { Router } from '@angular/router';
import { debounceTime, Observable } from 'rxjs';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  apiUrl = 'http://localhost:5000/products';

  pageNumber: number = 1;  // Numero della pagina corrente
  pageSize: number = 10;
  constructor(
    private productService: ProductService,
    private router: Router
  ) {}

   ngOnInit(): void {
    this.loadProducts();
  }

  // Caricamento dei prodotti con paginazione
  loadProducts(): void {
    console.log('Caricamento prodotti:', this.pageNumber, this.pageSize, this.products);
    this.productService.getProducts(this.pageNumber, this.pageSize).pipe(debounceTime(300)).subscribe(
      (data) => {
        console.log('Prodotti ricevuti:', data); // Aggiungi un log qui
        this.products = data;
      },
      (error) => {
        console.error('Errore nel caricamento dei prodotti:', error);
      }
    );
  }


  // Funzione per cambiare pagina
  onPageChange(newPageNumber: number): void {
    this.pageNumber = newPageNumber;
    this.loadProducts();
  }

  // Funzione per eliminare il prodotto
  deleteProduct(id: number): void {
    if (confirm('Sei sicuro di voler eliminare questo prodotto?')) {
      this.productService.deleteProduct(id).subscribe(
        () => {
          this.products = this.products.filter(product => product.id !== id);
          console.log('Prodotto eliminato con successo');
        },
        (error) => {
          console.error('Errore nell\'eliminazione del prodotto:', error);
        }
      );
    }
  }

  // Funzione per modificare il prodotto
  editProduct(productId: number): void {
    this.router.navigate(['/admin/products', productId]);
  }

  // Funzione per ottenere l'URL completo dell'immagine
  getFullImageUrl(imageUrl: string): string {
    if (!imageUrl.startsWith('http')) {
      return `https://localhost:44337/${imageUrl}`;
    }
    return imageUrl;
  }
}
