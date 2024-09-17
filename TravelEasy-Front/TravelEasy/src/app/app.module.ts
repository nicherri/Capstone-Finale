import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Assicurati che FormsModule e ReactiveFormsModule siano importati
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { QuillModule } from 'ngx-quill';
import { QuillTextEditorComponent } from './quill-text-editor/quill-text-editor.component';

@NgModule({
  declarations: [
    AppComponent,
    QuillTextEditorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule, // Per supportare ngModel
    ReactiveFormsModule, // Per i form reattivi
    HttpClientModule,
    QuillModule.forRoot() // Configura QuillModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA] // Aggiungi lo schema qui
})
export class AppModule { }
