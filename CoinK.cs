if (money >= 1000)
{
    moneyText.text = "+ " + (money/1000).ToString("0.0") + "k $";
}
else
{
    moneyText.text = "+ " + money.ToString() + " $";
}
