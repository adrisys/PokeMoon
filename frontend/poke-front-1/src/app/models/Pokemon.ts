import { Type } from "./Type";
import { PokeImage } from "./PokeImage";

export interface Pokemon {
    idpoke: number;
    pokename: string;
    hp: number;
    attack: number;
    defense: number;
    spattack: number;
    spdefense: number;
    speed: number;
    types: Type[];
    picURL: string;
    aspectRatio: number;

}