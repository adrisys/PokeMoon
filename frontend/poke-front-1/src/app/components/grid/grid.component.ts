import {
  ChangeDetectorRef,
  Component,
  HostListener,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subscription, firstValueFrom, lastValueFrom } from 'rxjs';
import { PokemonService } from 'src/app/services/pokemon.service';
import {
  BreakpointObserver,
  BreakpointState,
  Breakpoints,
} from '@angular/cdk/layout';
import { SearchService } from 'src/app/services/search-service.service';
import { Pokemon } from 'src/app/models/Pokemon';
import { PokeImage } from 'src/app/models/PokeImage';
import { SearchControlService } from 'src/app/services/search-control.service';
import { MatDialog } from '@angular/material/dialog';
import { PokemonDetailComponent } from '../pokemon-detail/pokemon-detail.component';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css'],
})
export class GridComponent implements OnInit, OnDestroy {
  cols: number = 3;
  numberFontSize: string = '0.75 vw';
  nameFontSize: string = '1 vw';
  offset: number = 0;
  pokesOnDB: number=-1;
  private searchSubscription: Subscription = new Subscription();
  typeColors = new Map([
    ['grass', '#4CAF50'], // Verde
    ['poison', '#9C27B0'], // Púrpura
    ['fire', '#F44336'], // Rojo
    ['water', '#2196F3'], // Azul
    ['bug', '#CDDC39'], // Lima
    ['normal', '#FFC107'], // Ámbar
    ['electric', '#FFEB3B'], // Amarillo
    ['ground', '#795548'], // Marrón
    ['fairy', '#E91E63'], // Rosa
    ['fighting', '#FF5722'], // Naranja
    ['psychic', '#9C27B0'], // Púrpura
    ['rock', '#607D8B'], // Azul Grisáceo
    ['steel', '#607D8B'], // Azul Grisáceo
    ['ice', '#03A9F4'], // Cian
    ['ghost', '#673AB7'], // Morado
    ['dragon', '#3F51B5'], // Índigo
    ['dark', '#000000'], // negro
    ['flying', '#03A9F4'], // Cian
  ]);
  pokemons: Pokemon[] = [];
  aspectRatio: number=0;
  private loading = false;

  constructor(
    private pokemonService: PokemonService,
    private breakpointObserver: BreakpointObserver,
    private searchService: SearchService,
    private searchControlService: SearchControlService,
    private cdr: ChangeDetectorRef,
    private dialog: MatDialog
  ) {
  }

  async ngOnInit() {
    this.pokesOnDB = await this.pokemonService.getHowMany();
    await this.LoadPokes(18);

    // Reacciona a los cambios en el tamaño de la pantalla
    this.breakpointObserver
      .observe([
        Breakpoints.XSmall,
        Breakpoints.Small,
        Breakpoints.Medium,
        Breakpoints.Large,
        Breakpoints.XLarge,
      ])
      .subscribe((result: BreakpointState) => {
        if (result.matches) {
          if (
            result.breakpoints[Breakpoints.XSmall] ||
            result.breakpoints[Breakpoints.Small]
          ) {
            this.cols = 1;
          } else if (result.breakpoints[Breakpoints.Medium]) {
            this.cols = 2;
          } else {
            this.cols = 3;
          }
        }
      });

    // Reacciona a los cambios en la búsqueda
    this.searchSubscription = this.searchService.searchAction$.subscribe(
      async (key: any) => {
          if(!/^\d+$/.test(key) ){
            try{
              key = await this.pokemonService.getPokemonId(key);
            }
            catch (error){
              console.error('Error al cargar el id del pokemon');
              console.error(error);
              this.searchControlService.changeState(true);
              setTimeout(() => {
                this.searchControlService.changeState(false);
              }, 1000);

              throw new Error('Error al cargar el id del pokemon');
            }
          }
        var id: number = Number.parseInt(key);
        if(id > this.pokesOnDB){       
          console.error('No existe un pokemon con id: ' + id);
          this.searchControlService.changeState(true);    
          setTimeout(() => {
            this.searchControlService.changeState(false);
          }, 1000);
          throw new Error('No existe un pokemon con id: ' + id);
        }
        if (this.offset < id) {
          await this.LoadPokes(Math.ceil(id / 9)*9-this.offset);
          this.cdr.detectChanges();
        }
        const element = document.getElementById('pokemon-' + key);
        if (!element) {
          console.error('Elemento no encontrado: ' + key);
          this.searchControlService.changeState(true);
          setTimeout(() => {
            this.searchControlService.changeState(false);
          }, 1000);
          throw new Error('Elemento no encontrado: ' + key);
        }

        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
        element.classList.add('highlight');

      }
    );
  }

// Carga pokemons
async LoadPokes(number: number) {
  // Comprueba si ya se está cargando
  if (this.loading) {
    return;
  }

  // Establece la variable de bloqueo a true
  this.loading = true;

  var pokemonsCargados: Pokemon[] = await this.pokemonService.getPokemonRange(number,this.offset);
  this.pokemons = [...this.pokemons, ...pokemonsCargados];
  this.offset += number;
  console.log('Se han cargado ' + number + ' pokemons');
  console.log('Pokemons cargados hasta el momento: ' + this.pokemons.length);

  // Establece la variable de bloqueo a false
  this.loading = false;
}

  ngOnDestroy(): void {
    this.searchSubscription.unsubscribe();
  }

  @HostListener('window:scroll', [])
  async onWindowScroll() {
    console.log('Scroll detectado');
    // Comprueba si el usuario se ha desplazado cerca del final de la página
    console.log('Scroll actual: ' + (window.innerHeight + window.scrollY));
    console.log(
      'Altura del documento: ' + document.documentElement.scrollHeight
    );
    if (
      window.innerHeight + window.scrollY >=
      document.documentElement.scrollHeight - 10
    ) {
      // Si es así, carga más pokemons 
      if (this.offset < this.pokesOnDB - 18) {
        await this.LoadPokes(18);
      }else if(this.offset < this.pokesOnDB){
        await this.LoadPokes(this.pokesOnDB-this.offset);
      }
    }
  }

  openDetails(pokemon: Pokemon) {
    this.dialog.open(PokemonDetailComponent, {
      data: {
        pokemon: pokemon
      }
    });
  }


calculateAspectRatio(event: Event): void {
  const img = event.target as HTMLImageElement;
  var aspectRatio = img.naturalWidth / img.naturalHeight;
  this.aspectRatio = aspectRatio;
}

}
