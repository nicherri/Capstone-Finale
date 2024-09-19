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
import { FormControl, FormGroup } from '@angular/forms';
import { Delta } from 'quill/core';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  productForm?: FormGroup;
  product: Product | undefined;
  productId: number;

  // Modelli per nuovi elementi
  newFAQ: FAQ = { id: 0, question: '', answer: '', productId: 0 };
  newBenefitDescription: string = '';
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

    this.productForm = new FormGroup({
      description: new FormControl(this.product?.description),  // Popola i campi form con il prodotto esistente
      benefits: new FormControl(this.product?.benefits),
      faq: new FormControl(this.product?.faqs)
    });

  }

  onSubmit() {
    console.log("cosa da cercare",this.productForm?.value);  // Qui vedrai i valori inseriti dall'utente
    // Puoi aggiungere la logica per salvare le modifiche
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
          image.imageUrl = loadedImage.imageUrl;
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
    if (!this.newBenefitDescription || !this.product || !this.product.id) {
      console.error('Descrizione o prodotto non definito');
      return;
    }

    const newBenefit: Benefit = {
      id: 0,  // Usa 0 come id temporaneo, sarà sovrascritto dal backend
      description: this.newBenefitDescription,
      productId: this.product.id // Associa al prodotto corrente
    };

    this.benefitService.addBenefit(newBenefit).subscribe((addedBenefit) => {
      if (this.product && this.product.benefits) {
        this.product.benefits.push(addedBenefit);  // Aggiungi il nuovo beneficio alla lista
      }
      this.newBenefitDescription = '';  // Resetta la descrizione
    }, error => {
      console.error('Errore durante l\'aggiunta del beneficio:', error);
    });
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
          // Crea un nuovo oggetto Image con la proprietà isCover
          const newImage: Image = {
            id: 0,  // Imposta un id provvisorio
            imageUrl: e.target.result,  // Usa l'URL base64 come immagine
            productId: this.product.id,
            isCover: false  // Imposta isCover su false per tutte le nuove immagini caricate
          };
          this.product.images.push(newImage);  // Aggiungi l'immagine al prodotto
        }
      };
      reader.readAsDataURL(file);  // Leggi il file come URL di dati
    });
  }


  setAsCover(image: Image): void {
    if (this.product) {
      // Imposta l'immagine selezionata come cover e deseleziona tutte le altre
      this.product.images.forEach(img => img.isCover = false);
      image.isCover = true;

      // Chiama il servizio per aggiornare l'immagine nel backend
      this.imageService.setImageAsCover(image.id).subscribe(() => {
        alert('Immagine di copertina impostata con successo');
      }, (error: any) => {  // Aggiungi il tipo 'any' esplicitamente
        console.error('Errore durante l\'impostazione dell\'immagine di copertina:', error);
      });
    }
  }





  removeImage(image: Image): void {
    if (this.product) {
      this.imageService.deleteImage(image.id).subscribe(() => {
        this.product!.images = this.product!.images.filter(img => img.id !== image.id);
        alert('Immagine rimossa con successo');
      }, error => {
        console.error('Errore durante la rimozione dell\'immagine:', error);
      });
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

  removeVideo(video: Video): void {
    if (this.product) {
      this.videoService.deleteVideo(video.id).subscribe(() => {
        // Rimuovi il video dalla lista localmente senza attendere il ricaricamento della pagina
        this.product!.videos = this.product!.videos.filter(v => v.id !== video.id);
        alert('Video rimosso con successo');
      }, error => {
        console.error('Errore durante la rimozione del video:', error);
      });
    }
  }



  onMediaUpload(event: any): void {
    const files: File[] = Array.from(event.target.files);

    files.forEach((file: File) => {
      const reader = new FileReader();
      const fileType = file.type.split('/')[0];  // 'image' o 'video'

      reader.onload = (e: any) => {
        if (this.product) {
          if (fileType === 'image') {
            const newImage: Image = {
              id: 0,
              imageUrl: e.target.result,  // Usa l'URL base64 come immagine
              productId: this.product.id,
              isCover: false
            };
            this.product.images.push(newImage);  // Aggiungi l'immagine al prodotto
          } else if (fileType === 'video') {
            const newVideo: Video = {
              id: 0,
              videoUrl: e.target.result,  // Usa l'URL base64 come video URL
              productId: this.product.id
            };
            this.product.videos.push(newVideo);  // Aggiungi il video al prodotto
          }
        }
      };
      reader.readAsDataURL(file);  // Leggi il file come URL di dati
    });
  }



  saveProductAndMedia(): void {
    if (this.product) {
      const updatedProduct: any = {
        id: this.product.id,
        title: this.product.title || '',
        subtitle: this.product.subtitle || '',
        description: this.product.description || '',
        price: this.product.price ? this.product.price : 0,
        numberOfPieces: this.product.numberOfPieces !== undefined ? this.product.numberOfPieces : 0,
        categoryId: this.product.categoryId,
        categoryName: this.product.categoryName || '',
        areaId: this.product.areaId || 0,
        areaName: this.product.areaName || '',
        shelvingId: this.product.shelvingId || 0,
        shelvingName: this.product.shelvingName || '',
        shelfId: this.product.shelfId || 0,
        shelfName: this.product.shelfName || '',
        faqs: this.product.faqs || [],
        benefits: this.product.benefits || [],
        reviews: this.product.reviews || [],
        relatedProducts: this.product.relatedProducts || []
      };

      const formData = new FormData();

      formData.append('product', new Blob([JSON.stringify(updatedProduct)], { type: 'application/json' }));

      // Aggiungi immagini selezionate al formData
      this.selectedImages.forEach(image => formData.append('images', image));

      // Aggiungi video selezionati al formData
      this.selectedVideos.forEach(video => formData.append('videos', video));

      // Chiama il servizio per salvare il prodotto e i media
      this.productService.updateProductWithMedia(this.product.id, updatedProduct, this.selectedImages, this.selectedVideos)
        .subscribe(() => {
          alert('Prodotto, immagini e video aggiornati con successo');
        }, error => {
          console.error('Errore durante l\'aggiornamento del prodotto e media:', error);
        });
    }
  }





}
