//start from the bottom where the main simulator loop is, and scroll up to find each function referenced


var c = document.getElementById('c');
var ctx = c.getContext('2d');

c.width = 800;
c.height = 400;
ctx.font = "8px Trebuchet MS";
ctx.textAlign = "center";


function randint(min, max) {
    var p = Math.random();
    p = Math.floor(p*(max-min))+min;
    return p;
}

function checkrange(p, rangestats) {
    var len = rangestats.length;
    var j=0;
    for (var i=0; i+1<len && rangestats[i+1]<=p; i++) {
        j++;
    }
    return j;
}

function assignregion() {
    var p=randint(0, regionstats[0]);
    var regionrange=checkrange(p, regionstats);
    var region = regionnames[regionrange];
    deathspercause[region][139]++;
    deathspercause["TOTAL"][139]++;
    return region;
}

function randomcoord(regionrect) {
    x = randint(0, regionrect[2])+regionrect[0];
    y = randint(0, regionrect[3])+regionrect[1];
    return [x, y];
}

function checkvalidcoord(xcoord, ycoord) {
    if (xcoord<800 && ycoord<400 && map[Math.floor(xcoord/20)][Math.floor(ycoord/20)]===0) {
        return 1;
    }
    else {
        return 0;
    }
}

function assigncoord(region) {
    regionrect = regionareas[region];
    var isvalid = 0;
    var coord = [0, 0];
    while (isvalid === 0) {
         coord = randomcoord(regionrect);
         isvalid = checkvalidcoord(coord[0], coord[1]+8);
    }    
    return coord;
}

function checkbornalive(region, coord) {
    var p=randint(0, stillbirthstats[region][0]);
    var stillbirthrange=checkrange(p, stillbirthstats[region]);
    if (stillbirthrange==0) {
        console.log('Stillborn');
        deathspercause[region][136]++;
        deathspercause[region][138]++;
        deathspercause["TOTAL"][136]++;
        deathspercause["TOTAL"][138]++;
        return 'Stillborn';
    }
    else {
        return 'None';
    }
}

function checkdeath(region, age) {
    var agerange=checkrange(age, ageranges);
    var p=randint(0, deathstats[region][agerange][0]-1);
    var causerange=checkrange(p, deathstats[region][agerange]);
    var cause=causenames[causerange];
    deathspercause[region][causerange]++;
    deathspercause["TOTAL"][causerange]++;
    return cause;
}



function aperson() {}


function updatedisplay() {
    ctx.clearRect(0, 0, 800, 400);
    ctx.fillStyle = "#990000";
    ctx.fillText('Time: '+yearcount, 400, 20);
    for (var i=0; i<n; i++) {
        if (people[i].cause==='None'){ 
            ctx.strokeStyle = "#FFFFFF";        
            ctx.beginPath();
            ctx.arc(people[i].coord[0], people[i].coord[1], 2, 0, 2*Math.PI);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0]-2, people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0]+2, people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.lineTo(people[i].coord[0]-2, people[i].coord[1]+10);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.lineTo(people[i].coord[0]+2, people[i].coord[1]+10);
            ctx.stroke();
        
        
        }
        else if(people[i].age>yearcount-5 && displaydead===1){
            ctx.strokeStyle = "#990000";
            ctx.beginPath();
            ctx.arc(people[i].coord[0], people[i].coord[1], 2, 0, 2*Math.PI);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0]-2, people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+2);
            ctx.lineTo(people[i].coord[0]+2, people[i].coord[1]+6);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.lineTo(people[i].coord[0]-2, people[i].coord[1]+10);
            ctx.moveTo(people[i].coord[0], people[i].coord[1]+6);
            ctx.lineTo(people[i].coord[0]+2, people[i].coord[1]+10);
            ctx.stroke();
            ctx.fillStyle = "#990000";
            ctx.fillText(people[i].cause, people[i].coord[0], people[i].coord[1]);
        }
    }
}

function update(person) {
    if (person.cause==='None'){
        person.cause=checkdeath(person.region, person.age);
        console.log(person.age, person.cause);
        if (person.cause==='None'){
            person.age++;
        }
        else {
            deathspercause[person.region][138]++;
            deathspercause["TOTAL"][138]++;
        }
    }
}

function topcauses(number, region) {
    var list = [];
    for(var i=0; i<=n; i++){
        for(var j=0; j<137; j++){
            if (deathspercause[region][j]===n-i) {
                list.push([causenames[j], n-i]);
            }            
        }
        if (list.length>=number){
            break;
        }
    }
    return list;
}

function reset() {
    people = [];
    yearcount = 1;
    alivecount = 0;
    displaydead = 1;
    for(var i=0; i<=140; i++) {
        deathspercause["TOTAL"][i]=0;
        deathspercause["AFR"][i]=0;
        deathspercause["AMR"][i]=0;
        deathspercause["EMR"][i]=0;
        deathspercause["EUR"][i]=0;
        deathspercause["SEAR"][i]=0;
        deathspercause["WPR"][i]=0;
    }
}

function finish() {
    yearcount--;
    displaydead = 0;
    updatedisplay();
    deathspercause["TOTAL"][137]=deathspercause["TOTAL"][139]-deathspercause["TOTAL"][138];
    deathspercause["AFR"][137]=deathspercause["AFR"][139]-deathspercause["AFR"][138];
    deathspercause["AMR"][137]=deathspercause["AMR"][139]-deathspercause["AMR"][138];
    deathspercause["EMR"][137]=deathspercause["EMR"][139]-deathspercause["EMR"][138];
    deathspercause["EUR"][137]=deathspercause["EUR"][139]-deathspercause["EUR"][138];
    deathspercause["SEAR"][137]=deathspercause["SEAR"][139]-deathspercause["SEAR"][138];
    deathspercause["WPR"][137]=deathspercause["WPR"][139]-deathspercause["WPR"][138];
    console.log(alivecount);
    ctx.fillStyle = "#990000";
    ctx.fillText('People still alive: '+deathspercause["TOTAL"][137], 400, 50);
    var topc = topcauses(10, "TOTAL");
    console.log(topc);
    ctx.fillText('Top causes of death:', 400, 70);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 400, 75+10*i);
    }
    topc = topcauses(10, "AFR");
    ctx.fillText('Africa:', 100, 220);
    ctx.fillText('People still alive: '+deathspercause["AFR"][137], 100, 250);
    ctx.fillText('Top causes of death:', 100, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 100, 275+10*i);
    }
    topc = topcauses(10, "AMR");
    ctx.fillText('Americas:', 220, 220);
    ctx.fillText('People still alive: '+deathspercause["AMR"][137], 220, 250);
    ctx.fillText('Top causes of death:', 220, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 220, 275+10*i);
    }
    topc = topcauses(10, "EMR");
    ctx.fillText('Eastern Mediterranean:', 340, 220);
    ctx.fillText('People still alive: '+deathspercause["EMR"][137], 340, 250);
    ctx.fillText('Top causes of death:', 340, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 340, 275+10*i);
    }
    topc = topcauses(10, "EUR");
    ctx.fillText('Europe:', 460, 220);
    ctx.fillText('People still alive: '+deathspercause["EUR"][137], 460, 250);
    ctx.fillText('Top causes of death:', 460, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 460, 275+10*i);
    }
    topc = topcauses(10, "SEAR");
    ctx.fillText('South East Asia:', 580, 220);
    ctx.fillText('People still alive: '+deathspercause["SEAR"][137], 580, 250);
    ctx.fillText('Top causes of death:', 580, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 580, 275+10*i);
    }
    topc = topcauses(10, "WPR");
    ctx.fillText('Western Pacific:', 700, 220);
    ctx.fillText('People still alive: '+deathspercause["WPR"][137], 700, 250);
    ctx.fillText('Top causes of death:', 700, 270);
    for(var i=1; i<=10; i++){
        ctx.fillText(i+'. '+topc[i-1][0]+' '+topc[i-1][1], 700, 275+10*i);
    }
}

function year() {
    alivecount = 0;
    for (var i=0; i<n; i++) {
        update(people[i]);
        if (people[i].cause==='None'){
            alivecount = alivecount + 1;
        }
    } 
    if (yearcount<80) {
        var blah = window.setTimeout(newyear, 100);
    }
    else {
        var blah = window.setTimeout(finish, 1000);
    }
    updatedisplay();
}

function newyear() {
    year();
    yearcount++;
}

var people = [];
var n = 1000;
var yearcount = 1;
var alivecount = 0;
var displaydead = 1;
//indexes following causenames, [136] 'Stillbirth', [137] 'None', [138] 'Total deaths', [139] 'Total all', 
var deathspercause = {
    "TOTAL":[],
    "AFR": [],
    "AMR": [],
    "EMR": [],
    "EUR": [],
    "SEAR": [],
    "WPR": []
};
for(var i=0; i<=140; i++) {
    deathspercause["TOTAL"][i]=0;
    deathspercause["AFR"][i]=0;
    deathspercause["AMR"][i]=0;
    deathspercause["EMR"][i]=0;
    deathspercause["EUR"][i]=0;
    deathspercause["SEAR"][i]=0;
    deathspercause["WPR"][i]=0;
}

function k() {
//initiation
    reset();
    for(var i=0; i<n; i++) {
        people[i] = new aperson();
        people[i].age = 0;
        people[i].region = assignregion();
        deathspercause[people[i].region][137]++;
        deathspercause["TOTAL"][137]++;
        people[i].coord = assigncoord(people[i].region);
        people[i].cause = checkbornalive(people[i].region, people[i].coord);
    }
    updatedisplay();
//main loop
    year();
    
//end
//don't insert stuff because setTimeout
}


