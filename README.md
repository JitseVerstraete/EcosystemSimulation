# EcosystemSimulation

## Introduction

In nature, an organisms genes determine it's ability to survive in it's environment. Organisms with better genes will have a bigger chance to survive long enough to reproduce and pass on their good genes. The gene pool of the organism will adapt to their current environment as the individuals with worse genes will not survive long enough to reproduce and pass on these genes.<br/>
<br/>
In this project, I want to simulate an organism with a certain set of traits determined by certain genes and see how different environments and situations affect the way the evolve over time. Not all genes will thrive in every environment, factors like predation, food availability and even the current population's size and genes.
In this project, I want to simulate an organism with a certain set of traits determined by certain genes and see how different environments and situations affect the way they evolve over time. Not all genes will thrive in every environment, factors like predation, food availability and even the current population's size and genes.<br/>

![alt text](/ReadmeImages/Sim.gif)

## Project Description
This simulation will work by updating all the organisms in the virtual space once every "time step". The time step is a value that defines how fast the simulation will go. 

### Organism Genes:
- Speed: defines how fast the organism will move
- Vision radius : defines how far the organism can see food, prey or predators around them.
### Organism Variables:
- Hunger: value between [0, 1]. Hunger rises by CalculatedHunger / MaxHunger every time step. If Hunger reaches 1, the organism dies. (CalculatedHunger is a value calculated based on the movement speed of the organism)
- Reproductive Urge: value between [0, 1]. R.U rises by 1 / MaxAge every time step. If R.U is bigger than hunger, the organism will look for a mate, else it will look for food. While looking for a mate, they will wander around until they find anther organism that is also looking for a mate.
- Age: value between [0, maxAge]. Age rises by 1 every time step. If an organism's age reaches maxAge, it dies.
<br/>
Every organism has two versions (aka alleles) of every gene, one from the "father" and one from the "mother". The actual value used to to set the maximum speed and vision range is the avergage of these two alleles. When reproducing, each parent give one of their alleles for each gen to the child. On top of this, every allele has a random chance to mutate which adds/substracts a small amount to/from the allele.
<br/>
<br/>
There are two different organisms, Blips and Predators. Blips eat static food in the world. Predators eat Blips for food. Blips will also run away from predators that are looking for blips to eat.

## Simulations
### First Simulation
I started with a baseline simulation to compare other simulations agains. A Simulation with only blips. I filled the world with a limited amount of food. <br/>
**Simulation Results:**
![alt text](/ReadmeImages/FirstSimulation.png)
**Explanation:** Because the food was not easily available anywhere, a lot of Blips go hungry and don't have the energy to reproduce. The Blip Population grows slowly but steadily. Both the Speed-Gene and the Vision-Gene go up steadily over time as well. The Blips with low vision range will not be able to see the food if it's not directly next to them, so Blips with this allele have a larger likelyhood of dying. Blips with the slower gene will lose the race to the food against faster Blips.

### Food Abundance Simulation
In this simulation I doubled the amount of food available, while leaving all other paramteres the same. <br/>
**Simulation Results:**
![alt text](/ReadmeImages/FoodAbundance.png)
**Explanation:** The most obvious consequence of increasing the food availability is the rapid growth in the Blip Population. In the begining the reproduce very quickly, after which the growth slows down and reaches an equilibrium. The Speed-Gene also increases rapidly in the beginning, this is probably due to the large number of blips being born, which creates a lot of chances for mutation to take place. Interestingly though, the Vision-Gene of the Blips went down over time. I don't see how having a low vision range could be an evolutionary advantage, but my guess is that it doesn't matter what the vision range is, because there are potential mates and food everywhere so even the blips with an extremely low vision range will still randomly stuble into food and mating partners.

### Predation Simulation
In this last simulation, I added a predator that hunts the blips for food. The predators work exactly the same way as the blips do, except the predators eat blips instead of the food on the ground. Blips will also run away when a predator is trying to eat them.
![alt text](/ReadmeImages/PredationSimulation.png)
**Explanation:** It was very hard to make Blips and the predators live in equilibrium with eachother for a long time. Either the predators failed to produce enough offstpring at the start and die off early, or the preadtors would grow slowly over time (because the time they spend mating is much higher that the blips, so growth is slow) unil they hit a critial mass and murder all of the Blips, after which they would all die of starvation :( . The genes also evolved how i expected them to, both the Blips and the Predators need to be fast enough to find food (and in the Blips' case, run away from the predators) so the Speed-Gene went up over time. Also note: when the blips were near extinction, the last few surviving blips are the ones with the best Speed-Gene, because they're good at running away (This is why the average speed of the blips goes up so drasticly when they're alost all dead.

## Conclusion & What's next?
There is a lot to be learned from running little simulations like these. I think it's a fun way to mimic nature-like behavior with relatively simple rules. I had a lot of fun just tweaking numbers and parameters to see what would happen in the simulation. There are a lot of things that I would like to add to this project for which i didn't have enough time given the current time constraints. <br/>
**Things I would like to add:**
- A gene that regulates the reproductive urge (instead of having it tied to the creatures lifespan)
- A gene that determines the amount of offspring a creature can have at once
- More complex movement behavior
- Difference between male and female organisms
- A gene that determines attractiveness/cammouflage (more attractive organisms will have an easier time finding a mate, but will also be easily spotted by predators)

## Bibliography
https://en.wikipedia.org/wiki/Heredity \
https://en.wikipedia.org/wiki/Population_genetics \
https://www.biologysimulations.com/simulations \
https://www.youtube.com/watch?v=r_It_X7v-1E&ab_channel=SebastianLague 
