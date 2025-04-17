using UnityEngine;

public class Ennemy : MonoBehaviour
{
    private int hp;
    private string name;
    private int difficulty;
    private int damage = 1;
    private int speed;

    Ennemy(int hp, string name, int difficulty, int damage, int speed)
    {
	this.hp = hp;
	this.name = name;
	this.difficulty = difficulty;
	this.damage = damage;
	this.speed = speed;
    }

    //Getter
    
    public int GetHp()
    {
	return this.hp;
    }

    public string GetName()
    {
	return this.name;
    }

    public int GetDifficulty()
    {
	return this.difficulty;
    }

    public int GetDamage()
    {
	return this.damage;
    }

    public int GetSpeed()
    {
	return this.speed;
    }

    //Setter

    public void SetHp(int hp)
    {
	this.hp = hp;
    }

    public void SetName(string name)
    {
	this.name = name;
    }

    public void SetDifficulty(int difficulty)
    {
	this.difficulty = difficulty;
    }

    public void SetDamage(int damage)
    {
	this.damage = damage;
    }

    public void SetSpeed(int speed)
    {
	this.speed = speed;
    }
    //Damge moyen method m*x+p +/-50%
    
    //Methode todo
    //damage taken
    //movement
    //attack
}
