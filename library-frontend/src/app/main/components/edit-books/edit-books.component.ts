import { Author } from './../../../core/models/author';
import { Component } from '@angular/core';
import { Book } from '../../../core/models/book';
import { Guid } from 'guid-typescript';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { BooksService } from '../../services/books.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { AuthorsService } from '../../services/authors.service';
import { MatDialog } from '@angular/material/dialog';
import { AddAuthorDialogComponent } from '../add-author-dialog/add-author-dialog.component';
import { forkJoin, map, mergeMap, Observable, of } from 'rxjs';

@Component({
  selector: 'app-edit-books',
  standalone: true,
  imports: [FormsModule, CommonModule, NgMultiSelectDropDownModule],
  templateUrl: './edit-books.component.html',
})
export class EditBooksComponent {

  public apiResources: string = "http://localhost:5000/Resources/"

  public book: Book | null = null;
  public isEditMode: boolean = false;
  public selectedImage: File | null = null;
  public imageChanged = false;

  public allAuthors : Author[] = [];
  public selectedAuthors : Author[] = [];
  public authorsToDelete : Author[] = [];
  public authorsToAdd : Author[] = [];
  public dropdownSettings : IDropdownSettings = {
    singleSelection: false,
    idField: 'id',
    textField: 'surname',
    itemsShowLimit: 5,
    allowSearchFilter: true,
    enableCheckAll: false
  };

  constructor(
    private activatedRoute: ActivatedRoute,
    private route: Router,
    private booksService: BooksService,
    private authorsService: AuthorsService,
    public dialog: MatDialog) 
  {

  }

  ngOnInit(): void{


    this.activatedRoute.paramMap.subscribe(params => {
      const bookId = params.get('id');

      if (bookId && Guid.isGuid(bookId)) {
        this.isEditMode = true;
        this.loadBook(Guid.parse(bookId));
      } 
      else {
        this.book = {
          id: Guid.createEmpty(),
          name: '',
          description: '',
          genre: '',
          isbn: '',
          authors: [],
        }
        this.loadAllAuthors();
      }
    })
  }

  onItemSelect(item: any) {
    this.authorsToAdd.push(item);
  }

  onItemDeSelect(item: any) {
    this.authorsToDelete.push(item);
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedImage = file
      this.imageChanged = true;
    }
  }

  openAddAuthorDialog(): void {
    const dialogRef = this.dialog.open(AddAuthorDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.allAuthors = [... this.allAuthors, result];
      }
    });
  }

  loadBook(bookId: Guid): void {
    this.booksService.get(bookId)
      .subscribe(book => {
        this.book = book;
        if(this.book.imagePath){
          this.loadImageFile(this.book.imagePath)
        }
        this.loadAllAuthors();
      })
  }

  loadImageFile(imageName: string){
    this.booksService.getImageFile(imageName)
      .subscribe(blob => {
        this.selectedImage = new File([blob], imageName, {type: blob.type})
      })
  }

  loadSelectedAuthors(){
    this.selectedAuthors = this.allAuthors.filter(author =>
      this.book?.authors.some(bookAuthor => bookAuthor.id === author.id)
    );  
  }

  loadAllAuthors(){
    this.authorsService.getAll()
      .subscribe(authors => {
        this.allAuthors = authors;
        this.loadSelectedAuthors();
      })
  }

  saveAuthors(bookId: Guid): Observable<any> {
    const addAuthors$ = this.authorsToAdd.map(author => 
      this.booksService.addAuthor(bookId, author.id)
    );

    const deleteAuthors$ = this.authorsToDelete.map(author => 
      this.booksService.deleteAuthor(bookId, author.id)
    );

    const actions$ = [...addAuthors$, ...deleteAuthors$];

    return actions$.length > 0 ? forkJoin(actions$) : of(null);
  }

  
  save() {
    console.log("save");
    
    if (this.book) {
      console.log("this.book");
      
      if (this.isEditMode) {

        console.log("edit mode");
        
        this.booksService.update(this.book, this.imageChanged ? this.selectedImage : null)
          .pipe(
            mergeMap(() => this.saveAuthors(this.book!.id))
          )
          .subscribe((id) => {
            console.log("admin books redirect");
            
            this.route.navigate(['/admin/books']);
          });
      } else {
        this.booksService.add(this.book, this.selectedImage)
          .pipe(
            mergeMap((id) => this.saveAuthors(id).pipe(
              map(() => id)
            ))
          )
          .subscribe((id) => {
            console.log("redirect admin books edit");
            this.route.navigate([`/admin/books/edit/${id}`]);
          });
      }
    }
  }
}
