using System;
using System.Threading.Tasks;
using Automatonymous;

namespace Example01
{
    class Program
    {
        static void Main(string[] args)
        {
            StateData stateData = new StateData();
            StateMachine stateMachine = new StateMachine();

            MyEventObserver myEventObserver = new MyEventObserver();
            MyStateObserver myStateObserver = new MyStateObserver();

            stateMachine.ConnectEventObserver(myEventObserver);
            stateMachine.ConnectStateObserver(myStateObserver);

            stateMachine.RaiseEvent(stateData, stateMachine.Start).Wait(); // go to s1
            stateMachine.RaiseEvent(stateData, stateMachine.Boom).Wait();  // go to s2
            stateMachine.RaiseEvent(stateData, stateMachine.ToSub).Wait(); // s2 does not handle ToSub
            stateMachine.RaiseEvent(stateData, stateMachine.Boom).Wait();  // go to s1
            stateMachine.RaiseEvent(stateData, stateMachine.ToSub).Wait(); // go to s21 --> Enter s2 is missing here!
            stateMachine.RaiseEvent(stateData, stateMachine.Quit).Wait();
        }
    }

    public class MyStateObserver : StateObserver<StateData>
    {
        public Task StateChanged(InstanceContext<StateData> context, State currentState, State previousState)
        {
            Console.WriteLine(new string(' ', 10) + $"StateChanged: {previousState} -> {currentState}");
            return Task.Delay(1);
        }
    }
    public class MyEventObserver : EventObserver<StateData>
    {
        public Task ExecuteFault(EventContext<StateData> context, Exception exception)
        {
            Console.WriteLine(new string(' ', 20) + $"ExecuteFault: {context.Event}");
            return Task.Delay(1);
        }

        public Task ExecuteFault<T>(EventContext<StateData, T> context, Exception exception)
        {
            Console.WriteLine(new string(' ', 20) + $"ExecuteFault: {context.Event}");
            return Task.Delay(1);
        }

        public Task PostExecute(EventContext<StateData> context)
        {
            Console.WriteLine(new string(' ', 20) + $"PostExecute: {context.Event}");
            return Task.Delay(1);
        }

        public Task PostExecute<T>(EventContext<StateData, T> context)
        {
            Console.WriteLine(new string(' ', 20) + $"PostExecute: {context.Event}");
            return Task.Delay(1);
        }

        public Task PreExecute(EventContext<StateData> context)
        {
            Console.WriteLine(new string(' ', 20) + $"PreExecute: {context.Event}");
            return Task.Delay(1);
        }

        public Task PreExecute<T>(EventContext<StateData, T> context)
        {
            Console.WriteLine(new string(' ', 20) + $"PreExecute: {context.Event}");
            return Task.Delay(1);
        }
    }

    public class StateData
    {
        public string CurrentSate { get; set; }
    }

    public class StateMachine : AutomatonymousStateMachine<StateData>
    {
        public StateMachine()
        {
            InstanceState(x => x.CurrentSate);

            SubState(() => s21, s2);

            Initially(When(Start).TransitionTo(s1));

            WhenEnterAny(x => x.Then(context => Console.WriteLine($"Enter {context.Instance.CurrentSate} ({context.Event}).")));
            WhenLeaveAny(x => x.Then(context => Console.WriteLine($"Leave {context.Instance.CurrentSate} ({context.Event}).")));

            During(s1,
                When(Boom).TransitionTo(s2),
                When(ToSub).TransitionTo(s21)
                );
            During(s2,
                When(Boom).TransitionTo(s1)
                );

            DuringAny(When(Quit).Finalize());

            Finally(x => x.Then(context => Console.WriteLine("We're done.")));

            OnUnhandledEvent(context => Task.Run(() => Console.WriteLine($"{context.Instance.CurrentSate} does not handle {context.Event}!")));
        }

        public State s1 { get; private set; }
        public State s2 { get; private set; }
        public State s21 { get; private set; }

        public Event Start { get; private set; }
        public Event Boom { get; private set; }
        public Event ToSub { get; private set; }
        public Event Quit { get; private set; }
    }
}
