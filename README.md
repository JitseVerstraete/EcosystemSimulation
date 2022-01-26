# EcosystemSimulation

## Introduction

In nature, an organisms genes determine it's ability to survive in it's environment. Organisms with better genes will have a bigger chance to survive long enough to reproduce and pass on their good genes. The gene pool of the organism will adapt to their current environment as the individuals with worse genes will not survive long enough to reproduce and pass on these genes.<br/>
<br/>
In this project, I want to simulate an organism with a certain set of traits determined by certain genes and see how different environments and situations affect the way the evolve over time. Not all genes will thrive in every environment, factors like predation, food availability and even the current population's size and genes.
In this project, I want to simulate an organism with a certain set of traits determined by certain genes and see how different environments and situations affect the way they evolve over time. Not all genes will thrive in every environment, factors like predation, food availability and even the current population's size and genes.<br/>

INSERT GIF HERE

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

## Simulations
I started with a baseline simulation to compare other simulations agains. A Simulation with only blips. 

## Conclusion

## What's next?

## Bibliography
https://en.wikipedia.org/wiki/Heredity \
https://en.wikipedia.org/wiki/Population_genetics \
https://www.biologysimulations.com/simulations \
https://www.youtube.com/watch?v=r_It_X7v-1E&ab_channel=SebastianLague 
