/*
*   Create By: Mario Cancel
*/
let lastId = 0;
//Function for assignment new unique id
export default function(prefix='id') {
    lastId++;
    return `${prefix}${lastId}`;
}