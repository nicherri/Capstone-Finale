import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

@Component({
  selector: 'app-quill-text-editor',
  templateUrl: './quill-text-editor.component.html',
  styleUrls: ['./quill-text-editor.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => QuillTextEditorComponent),
      multi: true
    }
  ]
})
export class QuillTextEditorComponent implements ControlValueAccessor {
  @Input() content: string = '';

  // Funzioni di registrazione di Angular Forms
  onChange: any = () => {};
  onTouched: any = () => {};

  // Configurazione della toolbar di Quill
  quillConfig = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ 'color': [] }, { 'background': [] }],
      [{ 'list': 'ordered' }, { 'list': 'bullet' }],
      [{ 'align': [] }],
      ['link', 'image', 'video'],
      ['code-block']
    ]
  };

  // Funzione chiamata da Angular quando il valore cambia
  writeValue(value: any): void {
    this.content = value;
  }

  // Funzione per registrare il cambiamento del valore
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  // Funzione per registrare il tocco
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  // Funzione chiamata quando il contenuto di Quill cambia
  onContentChanged(event: any): void {
    this.content = event.html;
    this.onChange(this.content);  // Notifica il cambiamento di contenuto
  }

  // Funzione chiamata quando l'editor viene sfocato
  onBlur(): void {
    this.onTouched();  // Notifica il tocco
  }
}
