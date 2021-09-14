public class MagicEffect 
 {
     public string Name {get;set;}
     public int SpellPower {get;set;}
     public float Duration {get;set;}
     // Damagetype ...etc ...
 
     public MagicEffect(string name, int spellpower, float duration)
     {
         Name = name;
         SpellPower = spellpower;
         Duration = duration;
     }
     
     public static MagicEffect Ignite = new MagicEffect("Ignite", 30, 0.5);
     public static MagicEffect Shock = new MagicEffect("Shock", 40, 1.5);
     public static MagicEffect AcidSpray = new MagicEffect("AcidSpray", 30, 0.5);
 }
 
 
 
 public void LoseHP()
 {
    _health -= MagicEffect.Ignite.SpellPower;


    if(_health <= 0)
    {
      _health = 0;

      Debug.Log ("Player_2 has died.");
    }
 }
