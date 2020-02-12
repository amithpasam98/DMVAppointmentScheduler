# DMVAppointmentScheduler

Hi,

I have few comments on my code which is added.

In Program.cs

Initial commit:
--------------
1) I have used orderBy(t => t.multiplier) in lines 44 and 55. Instead I can use it in line 41.
2) My initial thought is when the customer enters if there is no match for their type in teller, it is efficient to assingn a teller who has highest multiplier because anyways the multiplier is not counted against appointment time and the other teller with lowest multiplier can be asigned to customer whose type might be equal (This is just assumption)
3) But when I used OrderByDescending on unmatched tellerList (Line 44) the output is 645 minutes but with the code I have changed now is just 619 minutes.

Second commit:
--------------
I just restructed the code to improve execution time of program. Query on line 45 will run only when if condition is false. Also I have moved the OrberBy to the place of initialization (Line 41) so no need to run every time customer comes in.

Thanks for your time 
Amith Pasam

