using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class HpRegeneration : MonoBehaviour
 {
     [SerializeField]
     private int maxHealth = 5;
 
     [SerializeField]
     private float healthRegenerationRate = 1;
 
     [SerializeField]
     private float regenerationDelay = 1;
 
     private int currentHealth;
 
     private float lastDamagesTime = -1;
 
     private float regeneratedHealth;
 
     public bool IsDead
     {
         get { return currentHealth == 0; }
     }
 
     private void Awake()
     {
         currentHealth = maxHealth;
     }
 
     void Update()
     {
         if ( lastDamagesTime >= 0 && Time.time - lastDamagesTime >= regenerationDelay )
         {
             RegenerateHealth();
         }
     }
 
     public void RegenerateHealth()
     {
         regeneratedHealth += healthRegenerationRate * Time.deltaTime;
         int flooredRegeneratedHealth = Mathf.FloorToInt( regeneratedHealth );
         regeneratedHealth -= flooredRegeneratedHealth;
         Heal( flooredRegeneratedHealth );
     }
 
     public void Hurt( int damages )
     {
         if ( damages < 0 )
             throw new System.ArgumentException( "Damages must be greater or equal to 0", nameof( damages ) );
 
         if ( IsDead )
             return;
 
         currentHealth = Mathf.Clamp( currentHealth - damages, 0, maxHealth );
         regeneratedHealth = 0;
         lastDamagesTime = Time.time;
 
         if( IsDead )
         {
             // Die
         }
     }
 
     public void Heal( int amount )
     {
         if ( amount < 0 )
             throw new System.ArgumentException( "Healed amount must be greater or equal to 0", nameof( amount ) );
 
         currentHealth = Mathf.Clamp( currentHealth + amount, 0, maxHealth );
         if( currentHealth == maxHealth )
         {
             lastDamagesTime = -1;
             regeneratedHealth = 0;
             // Full health
         }
     }
 }
