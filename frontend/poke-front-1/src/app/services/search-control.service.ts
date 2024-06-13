import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchControlService {
  private errorSubject = new Subject<boolean>();

  errorAction$ = this.errorSubject.asObservable();

  changeState(state: boolean) {
    this.errorSubject.next(state);
  }
}