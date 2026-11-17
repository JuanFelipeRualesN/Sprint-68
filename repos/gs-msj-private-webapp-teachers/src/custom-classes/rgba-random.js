/**
 * Created by portiz on 9/26/18.
 */
/**
 * Generates a random RGBA color
 */
class RandomRGBA
{
     static randomRgba() {
         let o = Math.round, r = Math.random, s = 255;
        return 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',' + r().toFixed(1) + ')';
    }
}
export default RandomRGBA;