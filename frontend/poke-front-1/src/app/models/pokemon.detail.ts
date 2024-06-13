export class PokemonDetail {
    id: number;
    order: number;
    name: string;
    height: number;
    abilities: Ability[];
    species: Species;
    types: Type[];
    weight: number;
    sprites: Sprite;
    stats: Stat[];

    constructor() {
        this.id = -1;
        this.order = -1;
        this.name = '';
        this.height = -1;
        this.weight = -1;
        this.stats = [];
        this.sprites = new Sprite();
        this.species = new Species();
        this.abilities  = [];
        this.types = [];
    }
}

class Ability  {
    ability: {
        name: string;
    }

    constructor() {
        this.ability = {
            name: ''
        }
    }
}

class Species {
    url: string;
    constructor() {
        this.url = '';
    }
}

class Type {
    slot: number;
    type: {
        name: string;
    }
    constructor() {
        this.slot = -1;
        this.type = {
            name: ''
        }
    }
}

class Sprite {
    front_default: string;
    constructor() {
        this.front_default = '';
    }
}

class Stat {
    base_stat: number;
    stat: {
        name: string;
    }
    constructor() {
        this.base_stat = -1;
        this.stat = {
            name: ''
        }
    }
}