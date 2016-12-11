using GAF;
using GAF.Extensions;
using GAF.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanDemo
{
    public class GeneticResolver : IResolver
    {

        const int populationSize = 100;

        public async Task<IEnumerable<City>> 
            CalculateAsync(IEnumerable<City> cities)
        {
            //Each city is an object the chromosome is a special case as it needs 
            //to contain each city only once. Therefore, our chromosome will contain 
            //all the cities with no duplicates

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {

                var chromosome = new Chromosome();
                foreach (var city in cities)
                {
                    chromosome.Genes.Add(new Gene(city));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            //ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);


            //run the GA

            //ga.RunAsync(Terminate);

            var e = await ga.ExecuteAsync(Terminate);

            var fittest = e.Population.GetTop(1)[0];

            var q = fittest.Genes
                .Select(gene=> (City)gene.ObjectValue);

            return q;

            //foreach (var gene in fittest.Genes)
            //{
            //    Console.WriteLine(((City)gene.ObjectValue).Name);
            //}

        }

        public void Calculate(IEnumerable<City> cities)
        {
            //Each city is an object the chromosome is a special case as it needs 
            //to contain each city only once. Therefore, our chromosome will contain 
            //all the cities with no duplicates

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {

                var chromosome = new Chromosome();
                foreach (var city in cities)
                {
                    chromosome.Genes.Add(new Gene(city));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);
        }

        static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            foreach (var gene in fittest.Genes)
            {
                Console.WriteLine(((City)gene.ObjectValue).Name);
            }
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateDistance(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Distance: {2}", e.Generation, fittest.Fitness, distanceToTravel);

        }

        public static double CalculateFitness(Chromosome chromosome)
        {
            var distanceToTravel = CalculateDistance(chromosome);
            return 1 - distanceToTravel / 10000;
        }

        private static double CalculateDistance(Chromosome chromosome)
        {
            var distanceToTravel = 0.0;
            City previousCity = null;

            //run through each city in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentCity = (City)gene.ObjectValue;

                if (previousCity != null)
                {
                    var distance = previousCity.GetDistanceFromPosition(currentCity.Latitude,
                                                                        currentCity.Longitude);
                    distanceToTravel += distance;
                }

                previousCity = currentCity;
            }

            return distanceToTravel;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }

    }
}
