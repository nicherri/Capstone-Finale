import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../shared/models/product.model';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  apiUrl = 'http://localhost:5000/products';

  constructor(
    private productService: ProductService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe(
      (data) => {
        this.products = data;
      },
      (error) => {
        console.error('Errore nel caricamento dei prodotti:', error);
      }
    );
  }

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

  editProduct(productId: number): void {
    this.router.navigate(['/admin/products', productId]);
  }

  getFullImageUrl(imageUrl: string): string {
    if (!imageUrl.startsWith('http')) {
      return `http://localhost:5000/${imageUrl}`;
    }
    return imageUrl;
  }
}
