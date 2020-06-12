protected override void OnParticleCollision(GameObject hitTarget)
{
    base.OnParticleCollision(hitTarget);

    Agent hitAgent = hitTarget.GetComponent<Agent>();

    if (!CheckForFriendlyFire(hitAgent))
        hitAgent.TakeDamage(CurrentStage.Damage, DamageSource.Bullet);

}
