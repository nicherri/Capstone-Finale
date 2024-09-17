import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../shared/models/product.model';
import { BenefitService } from '../../core/services/benefit.service';
import { FAQService } from '../../core/services/faq.service';
import { ReviewService } from '../../core/services/review.service';
import { VideoService } from '../../core/services/video.service';
import { ImageService } from '../../core/services/image.service';
import { UserService } from '../../core/services/user.service';
import { Benefit } from '../../shared/models/benefit.model';
import { FAQ } from '../../shared/models/faq.model';
import { Review } from '../../shared/models/review.model';
import { Image } from '../../shared/models/image.model';
import { Video } from '../../shared/models/video.model';
import { User } from '../../shared/models/user.model';
import { Category } from '../../shared/models/category.model';
import { CategoryService } from '../../core/services/category.service';

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
  Benefit: Benefit = { id: 0, description: '', productId: 0 };
  newReview: Review = { id: 0, productId: 0, userId: 0, rating: 0, comment: '', createdAt: new Date(), reviewImages: [], reviewVideos: [] };
  categories: Category[] = [];
  selectedImages: File[] = [];
  selectedVideos: File[] = [];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private benefitService: BenefitService,
    private categoryService: CategoryService,
    private faqService: FAQService,
    private reviewService: ReviewService,
    private videoService: VideoService,
    private imageService: ImageService,
    private userService: UserService
  ) {
    this.productId = +this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadProduct();
    this.loadCategories();
  }

  // Carica il prodotto
  loadProduct(): void {
    this.productService.getProductById(this.productId).subscribe(
      (product) => {
        this.product = {
          ...product,
          benefits: product.benefits || [],
          faqs: product.faqs || []
        };

        this.loadBenefits();
        this.loadFAQs();

        if (this.product.reviews) {
          this.product.reviews.forEach((review) => {
            this.loadReviewMedia(review);
            this.loadUserForReview(review);
          });
        }
      },
      (error) => {
        console.error('Errore nel caricamento del prodotto:', error);
      }
    );
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe(
      (categories: Category[]) => {  // Specifica il tipo Category[]
        this.categories = categories;
      },
      (error: any) => {  // Specifica che `error` è di tipo `any`
        console.error('Errore nel caricamento delle categorie:', error);
      }
    );
  }


  // Carica utente per recensione
  loadUserForReview(review: Review): void {
    this.userService.getUserById(review.userId).subscribe(
      (user) => {
        review.user = user;
      },
      (error) => {
        console.error('Errore nel caricamento dell\'utente per la recensione:', error);
      }
    );
  }

  // Carica immagini e video per recensione
  loadReviewMedia(review: Review): void {
    review.reviewImages.forEach((image) => {
      this.imageService.getImageById(image.id).subscribe(
        (loadedImage) => {
          image.coverImageUrl = loadedImage.coverImageUrl;
        }
      );
    });
    review.reviewVideos.forEach((video) => {
      this.videoService.getVideoById(video.id).subscribe(
        (loadedVideo) => {
          video.videoUrl = loadedVideo.videoUrl;
        }
      );
    });
  }

  // Funzioni per gestire benefici
  addBenefit(): void {
    if (this.product) {
      this.Benefit.productId = this.product.id;
      this.benefitService.createBenefit(this.Benefit).subscribe(() => {
        alert('Beneficio aggiunto con successo');
        this.loadProduct(); // Ricarica il prodotto
      });
    }
  }

  loadBenefits(): void {
    if (this.product && this.product.id) {
      this.benefitService.getAllBenefits().subscribe(
        (benefits) => {
          if (this.product && benefits) {
            this.product.benefits = benefits.filter(b => b.productId === this.product?.id);
          }
        },
        (error) => {
          console.error('Errore nel caricamento dei benefici:', error);
        }
      );
    } else {
      console.warn('Prodotto non definito durante il caricamento dei benefici');
    }
  }



  updateBenefit(benefit: Benefit): void {
    this.benefitService.updateBenefit(benefit.id, benefit).subscribe(() => {
      alert('Beneficio aggiornato con successo');
    });
  }

  removeBenefit(benefitId: number): void {
    if (this.product && this.product.benefits) {
      this.benefitService.deleteBenefit(benefitId).subscribe(
        () => {
          if (this.product && this.product.benefits) { // Verifica se product e benefits sono definiti
            this.product.benefits = this.product.benefits.filter(b => b.id !== benefitId);
          }
        },
        (error) => {
          console.error('Errore durante la rimozione del beneficio:', error);
        }
      );
    } else {
      console.warn('Prodotto o lista dei benefici non definita durante la rimozione');
    }
  }



  // Funzioni per gestire FAQ
  addFAQ(): void {
    if (this.product) {
      this.newFAQ.productId = this.product.id;
      this.faqService.createFAQ(this.newFAQ).subscribe(() => {
        alert('FAQ aggiunta con successo');
        this.loadProduct(); // Ricarica il prodotto
      });
    }
  }

  loadFAQs(): void {
    if (this.product && this.product.id) {
      this.faqService.getAllFAQs().subscribe(
        (faqs) => {
          if (this.product && faqs) { // Verifica se product e faqs sono definiti
            this.product.faqs = faqs.filter(f => f.productId === this.product?.id);
          }
        },
        (error) => {
          console.error('Errore nel caricamento delle FAQ:', error);
        }
      );
    } else {
      console.warn('Prodotto non definito durante il caricamento delle FAQ');
    }
  }



  updateFAQ(faq: FAQ): void {
    this.faqService.updateFAQ(faq.id, faq).subscribe(() => {
      alert('FAQ aggiornata con successo');
    });
  }

  removeFAQ(faqId: number): void {
    if (this.product && this.product.faqs) {
      this.faqService.deleteFAQ(faqId).subscribe(
        () => {
          if (this.product && this.product.faqs) { // Verifica se product e faqs sono definiti
            this.product.faqs = this.product.faqs.filter(f => f.id !== faqId);
          }
        },
        (error) => {
          console.error('Errore durante la rimozione della FAQ:', error);
        }
      );
    } else {
      console.warn('Prodotto o lista delle FAQ non definita durante la rimozione');
    }
  }



  // Funzioni per gestire recensioni
  updateReview(review: Review): void {
    this.reviewService.updateReview(review.id, review).subscribe(() => {
      alert('Recensione aggiornata con successo');
    });
  }

  removeReview(reviewId: number): void {
    this.reviewService.deleteReview(reviewId).subscribe(() => {
      alert('Recensione rimossa con successo');
      this.loadProduct(); // Ricarica il prodotto
    });
  }

  // Funzioni per gestire immagini
  onImageUpload(event: any): void {
    const files: File[] = Array.from(event.target.files);
    files.forEach((file: File) => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        if (this.product) {
          // Crea un nuovo oggetto Image
          const newImage: Image = {
            id: 0,  // Imposta un id provvisorio
            coverImageUrl: e.target.result,  // Usa l'URL base64 come immagine
            productId: this.product.id
          };
          this.product.images.push(newImage);  // Aggiungi l'immagine al prodotto
        }
      };
      reader.readAsDataURL(file);  // Leggi il file come URL di dati
    });
  }


  saveImages(): void {
    if (this.product && this.selectedImages.length > 0) {
      const formData = new FormData();

      // Aggiungi ogni immagine selezionata al formData
      this.selectedImages.forEach((imageFile: File, index) => {
        formData.append(`images`, imageFile, imageFile.name);
      });

      // Invia il formData al server
      this.imageService.updateImage(this.product.id, formData).subscribe(() => {
        alert('Immagini aggiornate con successo');
      }, error => {
        console.error('Errore durante l\'aggiornamento delle immagini:', error);
      });
    }
  }



  removeImage(image: Image): void {
    if (this.product) {
      this.product.images = this.product.images.filter(img => img !== image);
    }
  }

  onVideoUpload(event: any): void {
    const files: File[] = Array.from(event.target.files);
    files.forEach((file: File) => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        if (this.product) {
          // Crea un nuovo oggetto Video
          const newVideo: Video = {
            id: 0,  // Imposta un id provvisorio
            videoUrl: e.target.result,  // Usa l'URL base64 come video URL
            productId: this.product.id
          };
          this.product.videos.push(newVideo);  // Aggiungi il video al prodotto
        }
      };
      reader.readAsDataURL(file);  // Leggi il file come URL di dati
    });
  }



  saveVideos(): void {
    if (this.product && this.selectedVideos.length > 0) {
      const formData = new FormData();

      // Aggiungi ogni video selezionato al formData
      this.selectedVideos.forEach((videoFile: File, index) => {
        formData.append(`videos`, videoFile, videoFile.name);
      });

      // Invia il formData al server
      this.videoService.updateVideo(this.product.id, formData).subscribe(() => {
        alert('Video aggiornati con successo');
      }, error => {
        console.error('Errore durante l\'aggiornamento dei video:', error);
      });
    }
  }



  // Funzione per aggiornare il prodotto
  updateProduct(): void {
    if (this.product) {
      const updatedProduct: any = {
        id: this.product.id,
        title: this.product.title || '',
        subtitle: this.product.subtitle || '',
        description: this.product.description || '',
        price: this.product.price ? this.product.price : 0,
        numberOfPieces: this.product.numberOfPieces !== undefined ? this.product.numberOfPieces : 0,
        categoryId: this.product.categoryId,
        categoryName: this.product.categoryName || '',  // Nome della categoria
      areaId: this.product.areaId,  // Assicurati che areaId non sia undefined
      areaName: this.product.areaName || ' ',  // Nome dell'area
      shelvingId: this.product.shelvingId,  // Assicurati che shelvingId non sia undefined
      shelvingName: this.product.shelvingName || '',  // Nome della scaffalatura
      shelfId: this.product.shelfId,  // Assicurati che shelfId non sia undefined
      shelfName: this.product.shelfName || '',
        faqs: this.product.faqs || [],
        benefits: this.product.benefits || [],
        reviews: this.product.reviews || [],
        images: this.product.images || [],
        videos: this.product.videos || []
      };

      console.log("Updated Product Payload:", updatedProduct);  // Controlla i valori inviati

      this.productService.updateProduct(
        this.product.id,
        updatedProduct,
        this.selectedImages, // Immagini selezionate
        this.selectedVideos  // Video selezionati
      ).subscribe(
        () => {
          alert('Prodotto aggiornato con successo');
        },
        (error) => {
          console.error('Errore durante l\'aggiornamento del prodotto:', error);
        }
      );
    }
  }



}
