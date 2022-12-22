# T120B165 Saityno taikomųjų programų projektavimas projektas "Hotelio"

## Sistemos aprašymas
Projekto tikslas – palengvinti viešbučio administratoriaus bei kitų viešbučio darbuotojų darbą 
bei pagerinti priimamų sprendimų kokybę, tikslumą. Veikimo principas – pačią platformą sudaro kelios dalys:
internetinė aplikacija, kuria naudosis viešbučio darbuotojai, administratorius, neregistruoti vartotojai bei API.
Darbininkas, norėdamas naudotis šia platforma, prisiregistruos (arba bus priregistruotas administratoriaus) 
prie internetinės aplikacijos ir galės susidaryti atlikti administracines veiklas susijusius su viešbučio kambarių nuomavimu, 
klientų priėmimu/išleidimų. Administratorius užsiimtų darbuotojų registravimu. 

## Aplikacijos wireframes ir atitinkantys langai

PDF failą su wireframes galite rasti [čia](wireframes.pdf) (wireframes.pdf).

## API dokumentacija

PDF failą su API dokumentacija galite rasti [čia](API_dokumentacija.pdf) (API_dokumentacija.pdf).

## Projekto išvados

1. Projektas įgyvendintas sėkmingai. Sukurtos pagrindinės objektų valdymo operacijos, veikianti sistema.
2. Naudojantis Azure Applicantion Service projekto serverinė bei klientinė pusė yra pasikiamos viešai tinkle.
3. Projekto serverinė dalis įgyvendinta pagal pagrindinius REST principus, kiekvienos operacijos rezultatą galima atpažinti pagal užklausos atsako kodą (response code).
4. Projekto klientinė dalis atlikta su React.JS turi responsive design, animacijas bei lengvai palaikomus elementus.
5. Projekto vartotojo sąsaja atitinka nubrėžtus wireframes.
