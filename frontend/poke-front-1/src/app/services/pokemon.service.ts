import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pokemon } from '../models/Pokemon';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PokemonService {
  private baseUrl = 'https://localhost:7161/api/Pokemon';

  constructor(private http: HttpClient) {}

  getPokemons(): Promise<Pokemon[]> {
    return firstValueFrom(this.http.get<Pokemon[]>(this.baseUrl));
  }

  getPokemonById(id: number): Promise<Pokemon> {
    return firstValueFrom(this.http.get<Pokemon>(`${this.baseUrl}/${id}`));
  }

  getPokemonRange(number: number, offset: number): Promise<Pokemon[]> {
    return firstValueFrom(
      this.http.get<Pokemon[]>(`${this.baseUrl}/range/${number}/${offset}`)
    );
  }

  getPokemonId(pokename: string): Promise<number> {
    return firstValueFrom(
      this.http.get<number>(`${this.baseUrl}/${pokename}/id`)
    );
  }

  getHowMany(): Promise<number>{
    return firstValueFrom(this.http.get<number>(`${this.baseUrl}/howMany`));
  }
}
