import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { AppComponent } from './app.component';
import { QuillEditorComponent } from './shared/quill-editor/quill-editor.component'; // Il tuo componente personalizzato
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';  // Nuova API per HttpClient

@NgModule({
  declarations: [
    AppComponent,
    QuillEditorComponent,

  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    RouterModule,
    QuillModule.forRoot(),  // Inizializzazione del Quill editor
  ],
  providers: [provideHttpClient(withFetch())],
  bootstrap: [AppComponent]
})
export class AppModule { }
