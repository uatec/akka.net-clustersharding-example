using Akka.Actor;
using System;

namespace ConsoleApplication
{
    public class MyActor : ReceiveActor
    {
        public MyActor()
        {
            Console.WriteLine("Actor Instantiated");
            
            ReceiveAny(o =>
            {
                Console.WriteLine("Received type: " + o.GetType().Name);
            });
        }
    }
}
