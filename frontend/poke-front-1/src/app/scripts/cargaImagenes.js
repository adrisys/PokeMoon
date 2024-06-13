



//Script para cargar imagenes y descargarlas en una carpeta local
// for (let i = 1; i <= 1302; i++) {
//     const url = `https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/${i.toString().padStart(3,'0')}.png`;
//     const path = `./src/assets/pokemons/${i}.png`;
//     downloadImage(url, path);
// }


const url = `https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/004.png`;
const path = `C:\\PokeMoon\\frontend\\poke-front-1\\src\\assets\\pokemons\\1.png`;
downloadImage(url, path);

//metodo para descargar una imagen dada un url y un path de destino
function downloadImage(url, path) {
    const fs = require('fs');
    const axios = require('axios');
axios({
    url,
    responseType: 'stream',
    headers: {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3'
    }
}).then((response) => {
    response.data.pipe(fs.createWriteStream(path));
});
}