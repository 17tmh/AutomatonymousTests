**Version: MassTransit 7.0.7**

Output (**correct**):

```
          StateChanged:  -> Initial (State)
Enter Initial (Initial.Enter (Event)).
                    PreExecute: Start (Event)
Leave Initial (Initial.Leave (Event)).
          StateChanged: Initial (State) -> s1 (State)
Enter s1 (s1.Enter (Event)).
                    PostExecute: Start (Event)
                    PreExecute: Boom (Event)
Leave s1 (s1.Leave (Event)).
          StateChanged: s1 (State) -> s2 (State)
Enter s2 (s2.Enter (Event)).
                    PostExecute: Boom (Event)
s2 does not handle ToSub (Event)!
                    PreExecute: Boom (Event)
Leave s2 (s2.Leave (Event)).
          StateChanged: s2 (State) -> s1 (State)
Enter s1 (s1.Enter (Event)).
                    PostExecute: Boom (Event)
                    PreExecute: ToSub (Event)
Leave s1 (s1.Leave (Event)).
          StateChanged: s1 (State) -> s21 (State)
Enter s21 (s2.Enter (Event)).
Enter s21 (s21.Enter (Event)).
                    PostExecute: ToSub (Event)
                    PreExecute: Quit (Event)
Leave s21 (s21.Leave (Event)).
Leave s21 (s2.Leave (Event)).
          StateChanged: s21 (State) -> Final (State)
Enter Final (Final.Enter (Event)).
We're done.
                    PostExecute: Quit (Event)
```