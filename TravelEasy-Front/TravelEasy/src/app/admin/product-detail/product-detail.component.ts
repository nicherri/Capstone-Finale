import { Component, OnInit } from '@angular/core';
	import { ActivatedRoute } from '@angular/router';
	import { ProductService } from '../../core/services/product.service';
	import { Product } from '../../shared/models/product.model';
	import { FAQ } from '../../shared/models/faq.model';
	import { Benefit } from '../../shared/models/benefit.model';
	import { Image } from '../../shared/models/image.model';
	import { Video } from '../../shared/models/video.model';
	import { Review } from '../../shared/models/review.model';
	import { CategoryService } from '../../core/services/category.service';
	import { AreaService } from '../../core/services/area.service';
	import { ShelvingService } from '../../core/services/shelving.service';
	import { ShelfService } from '../../core/services/shelf.service';
	import { Category } from '../../shared/models/category.model';
	import { Area } from '../../shared/models/area.model';
	import { Shelving } from '../../shared/models/shelving.model';
	import { Shelf } from '../../shared/models/shelf.model';

	@Component({
	  selector: 'app-product-detail',
	  templateUrl: './product-detail.component.html',
	  styleUrls: ['./product-detail.component.scss']
	})
	export class ProductDetailComponent implements OnInit {
	  product: Product | undefined;
	  productId: number;

	  // Modelli per nuovi elementi
	  newFAQ: FAQ = { id: 0, question: '', answer: '', productId: 0 };
	  newBenefit: Benefit = { id: 0, description: '', productId: 0 };
	  newImage: Image = { id: 0, coverImageUrl: '', productId: 0 };
	  newVideo: Video = { id: 0, videoUrl: '', altText: '', productId: 0 };
	  newReview: Review = { id: 0, rating: 0, comment: '', productId: 0, userId: 0, createdAt: new Date(), reviewImages: [], reviewVideos: [] };

	  categories: Category[] = [];
	  areas: Area[] = [];
	  shelvings: Shelving[] = [];
	  shelves: Shelf[] = [];

	  constructor(
		private route: ActivatedRoute,
		private productService: ProductService,
		private categoryService: CategoryService,
		private areaService: AreaService,
		private shelvingService: ShelvingService,
		private shelfService: ShelfService
	  ) {
		this.productId = +this.route.snapshot.paramMap.get('id')!;
	  }

	  ngOnInit(): void {
		this.loadProduct();
		this.loadCategories();
		this.loadAreas();
		this.loadShelvings();
		this.loadShelves();
	  }

	  loadProduct(): void {
		this.productService.getProductById(this.productId).subscribe(
		  (product) => {
			this.product = product;
		  },
		  (error) => {
			console.error('Errore nel caricamento del prodotto:', error);
		  }
		);
	  }

	  loadCategories(): void {
		this.categoryService.getAllCategories().subscribe(categories => {
		  this.categories = categories;
		});
	  }

	  loadAreas(): void {
		this.areaService.getAllAreas().subscribe(areas => {
		  this.areas = areas;
		});
	  }

	  loadShelvings(): void {
		this.shelvingService.getAllShelvings().subscribe(shelvings => {
		  this.shelvings = shelvings;
		});
	  }

	  loadShelves(): void {
		this.shelfService.getAllShelves().subscribe(shelves => {
		  this.shelves = shelves;
		});
	  }

	  // Funzioni per gestire immagini
	  updateImage(index: number, coverImageUrl: string): void {
		if (this.product) {
		  this.product.images[index].coverImageUrl = coverImageUrl;
		}
	  }

	  removeImage(index: number): void {
		if (this.product) {
		  this.product.images.splice(index, 1);
		}
	  }

	  addImage(): void {
		if (this.product) {
		  this.newImage.productId = this.product.id;
		  this.product.images.push({ ...this.newImage });
		  this.newImage = { id: 0, coverImageUrl: '', productId: this.product.id };
		}
	  }

	  // Funzioni per gestire benefici
	  updateBenefit(index: number, description: string): void {
		if (this.product) {
		  this.product.benefits[index].description = description;
		}
	  }

	  removeBenefit(index: number): void {
		if (this.product) {
		  this.product.benefits.splice(index, 1);
		}
	  }

	  addBenefit(): void {
		if (this.product) {
		  this.newBenefit.productId = this.product.id;
		  this.product.benefits.push({ ...this.newBenefit });
		  this.newBenefit = { id: 0, description: '', productId: this.product.id };
		}
	  }

	  // Funzioni per gestire video
	  updateVideo(index: number, videoUrl: string, altText: string): void {
		if (this.product) {
		  this.product.videos[index].videoUrl = videoUrl;
		  this.product.videos[index].altText = altText;
		}
	  }

	  removeVideo(index: number): void {
		if (this.product) {
		  this.product.videos.splice(index, 1);
		}
	  }

	  addVideo(): void {
		if (this.product) {
		  this.newVideo.productId = this.product.id;
		  this.product.videos.push({ ...this.newVideo });
		  this.newVideo = { id: 0, videoUrl: '', altText: '', productId: this.product.id };
		}
	  }

	  // Funzioni per gestire FAQ
	  updateFAQ(index: number, question: string, answer: string): void {
		if (this.product) {
		  this.product.faqs[index].question = question;
		  this.product.faqs[index].answer = answer;
		}
	  }

	  removeFAQ(index: number): void {
		if (this.product) {
		  this.product.faqs.splice(index, 1);
		}
	  }

	  addFAQ(): void {
		if (this.product) {
		  this.newFAQ.productId = this.product.id;
		  this.product.faqs.push({ ...this.newFAQ });
		  this.newFAQ = { id: 0, question: '', answer: '', productId: this.product.id };
		}
	  }

	  // Funzioni per gestire recensioni
	  updateReview(index: number, rating: number, comment: string): void {
		if (this.product) {
		  this.product.reviews[index].rating = rating;
		  this.product.reviews[index].comment = comment;
		}
	  }

	  removeReview(index: number): void {
		if (this.product) {
		  this.product.reviews.splice(index, 1);
		}
	  }

	  addReview(): void {
		if (this.product) {
		  this.newReview.productId = this.product.id;
		  this.newReview.createdAt = new Date();
		  this.product.reviews.push({ ...this.newReview });
		  this.newReview = { id: 0, rating: 0, comment: '', productId: this.product.id, userId: 0, createdAt: new Date(), reviewImages: [], reviewVideos: [] };
		}
	  }

	  // Funzione per salvare tutte le modifiche
	  saveChanges(): void {
		if (this.product) {
		  this.productService.updateProduct(this.product.id, this.product).subscribe(
			() => {
			  console.log('Prodotto aggiornato con successo');
			},
			(error) => {
			  console.error('Errore durante l\'aggiornamento del prodotto:', error);
			}
		  );
		}
	  }
	}
