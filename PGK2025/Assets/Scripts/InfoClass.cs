using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoClass
{
    public string name;
    public string description;

    public InfoClass(string name)
    {
        switch (name)
        {
            case "Jasnoœæ:":
                this.name = "Jasnoœæ";
                this.description = "Przekszta³cenie punktowe, polegaj¹ce na liniowej zmianie jasnoœci ka¿dego piksela obrazu. Wartoœci dodatnie powoduj¹ rozjaœnienie obrazu, natomiast ujemne — jego przyciemnienie. Podczas stosowania tego przekszta³cenia nale¿y pamiêtaæ o normalizacji wartoœci, aby unikn¹æ przekroczenia dopuszczalnego zakresu jasnoœci";


                break;
            case "Kontrast:":
                this.name = "Kontrast";
                this.description = "Przekszta³cenie punktowe, polegaj¹ce na liniowej zmianie kontrastu obrazu poprzez skalowanie wartoœci jasnoœci pikseli. Wartoœci wiêksze zwiêkszaj¹ kontrast, a mniejsze go zmniejszaj¹. Nale¿y pamiêtaæ o normalizacji, aby zachowaæ poprawny zakres wartoœci.";
                break;
            case "Gamma:":
                this.name = "Gamma";
                this.description = "Przekszta³cenie punktowe, polegaj¹ce na nieliniowej modyfikacji jasnoœci pikseli za pomoc¹ funkcji potêgowej. Wartoœci gamma mniejsze od 1 rozjaœniaj¹ obraz (szczególnie w ciemnych obszarach), natomiast wiêksze od 1 powoduj¹ jego przyciemnienie.";
                break;
            case "Nasycenie:":
                this.name = "Nasycenie";
                this.description = "Przekszta³cenie punktowe, polegaj¹ce na modyfikacji nasycenia barw obrazu poprzez zmianê intensywnoœci sk³adowych kolorów. Zwiêkszenie nasycenia powoduje wzmocnienie kolorów, a jego zmniejszenie prowadzi do ich os³abienia a¿ do obrazu w skali szaroœci.";
                break;
            default:
                this.name = "Nieznany filtr";
                this.description = "Brak opisu dla tego filtra.";
                break;
        }
    }
}
