import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthorsService } from '../../services/authors.service';
import { Author } from '../../../core/models/author';
import { Guid } from 'guid-typescript';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, NativeDateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-add-author-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDatepickerModule],
  templateUrl: './add-author-dialog.component.html'
})
export class AddAuthorDialogComponent {
  author : Author = { id: Guid.createEmpty(), name: '', surname: '', birthday: new Date(), country: '' };

  constructor(public dialogRef: MatDialogRef<AddAuthorDialogComponent>,
    private authorsService: AuthorsService
  ) {}

  onSubmit(): void {
    this.authorsService.add(this.author)
      .subscribe((id) => {
        console.log("author added", id);
        this.author.id = id;
        this.dialogRef.close(this.author);
      });
  }
}
