import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-quill-editor',
  templateUrl: './quill-editor.component.html',
  styleUrls: ['./quill-editor.component.scss']
})
export class QuillEditorComponent {
  @Input() content: string = ''; // Testo iniziale per l'editor
  @Output() contentChange = new EventEmitter<string>(); // Emette il contenuto aggiornato

  quillModules = {
    toolbar: [
      ['bold', 'italic', 'underline', 'strike'],        // Grassetto, corsivo, sottolineato
      ['blockquote', 'code-block'],                     // Citazioni e blocchi di codice
      [{ 'list': 'ordered'}, { 'list': 'bullet' }],     // Elenchi ordinati e puntati
      [{ 'script': 'sub'}, { 'script': 'super' }],      // Apice e pedice
      [{ 'indent': '-1'}, { 'indent': '+1' }],          // Indentazioni
      [{ 'direction': 'rtl' }],                         // Direzione del testo
      [{ 'size': ['small', false, 'large', 'huge'] }],  // Dimensioni del testo
      [{ 'header': [1, 2, 3, 4, 5, 6, false] }],        // Intestazioni
      [{ 'color': [] }, { 'background': [] }],          // Colori
      [{ 'font': [] }],                                 // Font
      [{ 'align': [] }],                                // Allineamenti
      ['link', 'image', 'video'],                       // Link, immagini e video
      ['clean']                                         // Pulsante per pulire la formattazione
    ]
  };

  // Quando il contenuto cambia, lo emettiamo al componente padre
  onContentChanged(event: any): void {
    this.contentChange.emit(event.html); // Emette il nuovo contenuto HTML
  }
}
