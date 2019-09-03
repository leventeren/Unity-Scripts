string seed = Time.time.ToString();
System.Random random = new System.Random(seed.GetHashCode());
Color randomColor = new Color(
    (float)random.Next(0, 255),
    (float)random.Next(0, 255),
    (float)random.Next(0, 255)
);
