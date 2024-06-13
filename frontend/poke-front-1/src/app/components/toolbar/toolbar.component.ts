import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { SearchControlService } from 'src/app/services/search-control.service';
import { SearchService } from 'src/app/services/search-service.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent {
  private searchControlSubscription: Subscription = new Subscription();
   showError: boolean=false;

  constructor(private searchService: SearchService, private searchControlService: SearchControlService) { } 

  buscarPokemon(id: string) {
    this.searchService.search(id);

  }

  ngOnInit(){
    this.searchControlSubscription = this.searchControlService.errorAction$.subscribe(
      (value => this.showError=value)
    );
  }
}