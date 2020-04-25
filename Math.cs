// prints 10
Debug.Log(Mathf.Abs(-10));

// min and max value
private float xMin = -1.0f, xMax = 1.0f;
private float timeValue = 0.0f;

float xValue = Mathf.Sin(timeValue * 5.0f);
float xPos = Mathf.Clamp(xValue, xMin, xMax);
transform.position = new Vector3(xPos, 0.0f, 0.0f);
timeValue = timeValue + Time.deltaTime;
if (xValue > Mathf.PI * 2.0f)
    {
        timeValue = 0.0f;
    }
    
    
// rotate max 180
Mathf.MoveTowards(transform.rotation.z,180,1);
Mathf.MoveTowards(transform.rotation.z,0,1);

// smooth
float a = 0f;
float b = 100f;  
void Update()
{
    a = Mathf.Lerp( a, b, 0.5f );
}



Mathf.Clamp(birinciparametre_float,ikinciparametre_float,ucuncuparametre_float)

Clamp metodu üç parametre alır ve birinci parametrenin ikinci parametreden büyük, üçüncü parametreden küçük olmasını garantiler. Örneğin kullanıcıdan alınan bir ınput'un değerini kısıtlayalım ve kullanıcı geri tuşuna basmış olsa bile negatif değer almasını önleyelim.


void Update()
    {
        float x=Input.GetAxis("Horizontal");
        Debug.Log("X="+x+ " CLAMP SONRASI=" + Mathf.Clamp(x, 0, 1));
    }


Mathf.Abs(birinciparametre_float)
Abs metodu içine girilen parametrenin mutlak değerini döndürür.


void Update()
    {
        float negatif_Sayi = -5;
        Debug.Log(Mathf.Abs(negatif_Sayi));
    }


Mathf.Lerp(birinciparametre_float,ikinciparametre_float,ucuncuparametre_float)

Bu metod bir işlemi yumuşatmaya yarıyor burada yumuşatmadan kasıt, bir değerden başka bir değere hızlıca değil adım adım geçiştir.
Metodun birinci parametresi A değeri olsun, ikinci parametre B değeri olsun bu durumda üçüncü parametre fonksiyonun döndürdüğü değerin A ile B arasında hangi noktada olacağını belirtir.

Örneğin
Mathf.Lerp(0,100,0.5f) dersek burada dönüş 50 olacaktır. 0.5 yerine 0.75 yazsaydık dönüş değeri 75 olacaktı.

Mathf.Lerp(50,100,0.5f) dersek burada dönüş 75 olacaktır. 50 ile 100'ün ortasındaki değeri döndürecektir.

Basit bir örnek yazalım.
Sahnemizin 0,0,0 noktasının sağına ve soluna eşit mesafede iki nesne yerleştirelim.
Aşağıdaki gibi:

Daha sonra bu nesnelerin iki farklı yolla 0,0,0 noktasına geçmesini sağlayalım birinci yolda nesnemiz çok hızlı bir şekilde bu noktaya geçecektir.
İkinci yolda ise hareketi lerp metodu ile yavaşlatacağız.


public GameObject obje1, obje2;
    void Update()
    {
        obje1.transform.position = new Vector3(0, 0, 0);
        obje2.transform.position = Vector3.Lerp(obje2.transform.position, new Vector3(0,0,0), 0.5f);
    }


Burada Obje 1 anında 0,0,0 noktasına geçecektir. Obje2 ise mesafenin yarısını hızlıca gittikten sonra diğer yarısını yavaş bir şekilde gidecektir.

Mathf.LerpAngle(birinciparametre_float,ikinciparametre_float,ucuncuparametre_float)

Bu metod da tıpkı Lerp gibi çalışır tek farkı açısal işlemleri yapmasıdır. İkinci parametresi 360'dan büyük olduğu zaman onu 360'a tamamlamaya çalışır. Bu metod hız göstergesi yapımında kullanılabilir.


public GameObject obje1;
    void Update()
    {
        obje1.transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(0, 90, Time.time), 0);
    }


Burada obje1 y ekseninde 90 dereceye kadar dönecektir. Bu işlem smooth bir şekilde icra edilecektir.

Mathf.Infinity

Infinitiy metodu adından da anlaşılacağı üzere +sonsuz değerini döndürür. Belli işlemleri sonsuza kadar yapmak için sonsuz döngü yerine kullanılabilir.


Mathf.PI

Örneğin bir dairenin alanını hesaplamanız gerekiyor ve Pİ sayısına ihtiyacınız var. Fakat hassas bir sonuç için sadece 3.14 yazmak işinizi görmüyor. Böyle bir durumda Mathf.PI kullanarak Pİ sayısına erişebilirsiniz.


 void Update()
    {
        float yariCap = 15.2f;
        Debug.Log("Dairenin Alanı:" + Mathf.PI * yariCap * yariCap);
    }


Trigonometrik İfadeler

Mathf.Cos(parametre): İçine parametre olarak aldığı radyan cinsinden açının cosinüs değerini verir.
Mathf.Sin(parametre): İçine parametre olarak aldığı radyan cinsinden açının sinüs değerini verir.
Mathf.Tan(parametre): İçine parametre olarak aldığı radyan cinsinden açının tanjant değerini verir.
Mathf.Acos(parametre): İçine parametre olarak aldığı radyan cinsinden açının arccosinüs değerini verir.
Mathf.Asin(parametre): İçine parametre olarak aldığı radyan cinsinden açının arcsinüs değerini verir.
Mathf.Atan(parametre): İçine parametre olarak aldığı radyan cinsinden açının arctanjant değerini verir.
