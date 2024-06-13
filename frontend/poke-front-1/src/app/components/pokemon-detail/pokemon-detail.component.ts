import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Pokemon } from 'src/app/models/Pokemon';


@Component({
  selector: 'app-pokemon-detail',
  templateUrl: './pokemon-detail.component.html',
  styleUrls: ['./pokemon-detail.component.css']
})
export class PokemonDetailComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: {pokemon: Pokemon}) { 
  }

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
  

}
