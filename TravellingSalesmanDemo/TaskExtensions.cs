using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanDemo
{
    public static class TaskExtensions
    {
        public async static Task<GaEventArgs> ExecuteAsync(this GeneticAlgorithm myLibrary, TerminateFunction terminate)
        {
            var tcs = new TaskCompletionSource<GaEventArgs>();

            GeneticAlgorithm.RunCompleteHandler handler = default(GeneticAlgorithm.RunCompleteHandler);

            handler += (sender, args) =>
            {
                try
                {
                    tcs.TrySetResult(args);

                }

                finally
                {
                    myLibrary.OnRunComplete -= handler;
                }

            };

            myLibrary.OnRunComplete += handler;

            myLibrary.RunAsync(terminate);

            // Returns a Task object that allows to wait for the operation
            // to complete.
            return await tcs.Task;
        }
    }
}
