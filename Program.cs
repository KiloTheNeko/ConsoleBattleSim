using System.Security.Cryptography.X509Certificates;

namespace ConsoleBattleWars
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArgumentNullException.ThrowIfNull(args);

            Console.WriteLine("Hello, World!");

            Warrior Herakles = new("Herakles");
            Mage Merlin = new("Merlin");
            //Unit.Combat(Herakles, Merlin);
            Battallion Warriors = new Battallion(Herakles, 10, "Warriors");
            Battallion Defenders = new Battallion(Merlin, 10, "Mages");
            //Battallion.Combat(Defenders, Warriors);

            List<Battallion> Battallions = new List<Battallion>();
            Battallions.Add(Warriors);
            Battallions.Add(Defenders);
            Army Army = new(Battallions, "Army");
            Army Army2 = new(Battallions, "Army2");
            Army.battle(Army, Army2);
        }
        class Hero
        {
            public string Name { get; set; }
            public int atk { get; set; }
            public int def { get; set; }
        }
        class Army
        {
            List<Battallion> Battallions { get; set; }
            Hero Leader { get; set; }
            int Health { get; set; }
            int Speed { get; set; }
            string Name { get; set; }
            public Army(List<Battallion> _Battallion, string _name)
            {
                this.Name = _name;
                this.Battallions = _Battallion;
                foreach (Battallion battallion in _Battallion)
                {
                    this.Health += battallion.Health;
                    this.Speed += battallion.Speed;
                }


            }
            public void Attack(Army Defender, Army Attacker)
            {
                int damage;
                if (Attacker.Leader != null)
                {
                    damage = Attacker.Leader.atk;
                }
                else
                {
                    damage = 0;
                }
                foreach (Battallion battallion in this.Battallions)
                {
                    damage += battallion.Calculate_attack();
                }
                foreach (Battallion battallion in Defender.Battallions)
                {
                    if (Defender.Leader != null) { damage -= battallion.Defense + Defender.Leader.def; }
                    else { damage -= battallion.Defense; }
                }
                if (damage <= 0)
                {
                    damage = 1;
                }

                if (Defender.Battallions.Count == 0)
                {
                    damage = 1;
                }
                else
                {
                    damage = damage / Defender.Battallions.Count;
                }

                for (int i = Defender.Battallions.Count - 1; i >= 0; i--)
                {
                    Battallion battallion = Defender.Battallions[i];
                    battallion.Health -= damage;
                    Console.WriteLine($"{Attacker.Name} does {damage} damage to {battallion.Name}");
                    if (battallion.Health <= 0)
                    {
                        battallion.Unit_amount = 0;
                        Console.WriteLine($"{battallion.Name} Perished.");
                        Defender.Battallions.RemoveAt(i);
                    }
                }
            }
            public void battle(Army Defender, Army Attacker)
            {
                while (Defender.Battallions.Count > 0 && Attacker.Battallions.Count > 0)
                {
                    Console.WriteLine($"{Attacker.Name} attacks {Defender.Name}");
                    Attacker.Attack(Defender, Attacker);
                    Console.WriteLine($"Defender {Defender.Battallions.Count} units");
                    Console.WriteLine($"{Defender.Name} counterattacks {Attacker.Name}");
                    Defender.Attack(Attacker, Defender);
                    Console.WriteLine($"Defender has {Attacker.Battallions.Count} units");
                }
                if (Defender.Battallions.Count <= 0)
                {
                    Console.WriteLine($"{Defender.Name} Loses.");
                }
                else if (Attacker.Battallions.Count <= 0)
                {
                    Console.WriteLine($"{Attacker.Name} Loses.");
                }
                Console.WriteLine("Battle is over");
            }
        }
        class Battallion
        {
            public Battallion(Unit _Units, int _Unit_amount, string _name)
            {
                this.Units = _Units;
                this.Name = _name;
                this.Defense = _Units.Defense * _Unit_amount;
                this.Health = _Units.Health * _Unit_amount;
                this.Speed = _Units.Speed * _Unit_amount;
                this.Unit_amount = _Unit_amount;
                Console.WriteLine($"{this.Name} has been created. with {_Unit_amount} {_Units.Name} giving {this.Atk} attack, {this.Defense} defense, {this.Health} health and {this.Speed} speed");
            }

            public int Atk { get; set; }

            public int Defense { get; set; }

            public int Health { get; set; }

            public string Name { get; set; }

            public int Speed { get; set; }


            Unit Units { get; set; }
            public int Unit_amount { get; set; }

            static public void Combat(Battallion Defender, Battallion Attacker)
            {
                while (Defender.Health > 0 && Attacker.Health > 0)
                {
                    Console.WriteLine($"{Attacker.Name} attacks {Defender.Name}");
                    Attacker.Attack(Defender, Attacker);
                    Console.WriteLine($"Defender has: {Defender.Health} health and {Defender.Unit_amount} units");
                    Console.WriteLine($"{Defender.Name} counterattacks {Attacker.Name}");
                    Defender.Attack(Attacker, Defender);
                    Console.WriteLine($"Defender has: {Attacker.Health} health and {Attacker.Unit_amount} units");
                }
                if (Defender.Health <= 0 && Defender.Unit_amount <= 0)
                {
                    Console.WriteLine($"{Defender.Name} Loses.");
                }
                else if (Attacker.Health <= 0 && Attacker.Unit_amount <= 0)
                {
                    Console.WriteLine($"{Attacker.Name} Loses.");
                }
            }

            public void Attack(Battallion Defender, Battallion Attacker)
            {
                int damage = Attacker.Units.Calculate_attack();
                damage *= Attacker.Unit_amount;
                Console.WriteLine($"rolled {damage}");
                if (damage <= 0)
                {
                    damage = 1;
                }
                Defender.Health -= damage;
                Console.WriteLine($"{Attacker.Name} does {damage} damage to {Defender.Name}");
                if (Defender.Unit_amount > 1)
                {
                    Defender.Defense = Defender.Units.Defense;
                    Defender.Unit_amount -= damage/Defender.Units.Health;
                    Defender.Speed = Defender.Speed;
                }
                if (Defender.Health <= 0) { Defender.Unit_amount = 0; }

            }
            public int Calculate_attack()
            {
                int damage = Units.Calculate_attack();
                damage *= Unit_amount;
                return damage;
            }
        }

        class Mage : Unit
        {
            public Mage(string name): base(name)
            {
                this.Name = name;
                this.Atk = 3;
                this.Defense = 2;
                this.Health = 10;
                this.Variance = 6;
                this.Speed = 2;
                this.Missiles = 3;
            }

            int Missiles { get; set; }

            public override void Attack(Unit Defender, Unit Attacker)
            {
                Random random = new();
                int negative = random.Next(0, 1) * 2 - 1;
                int damage = Atk + (random.Next(0, this.Variance) * negative) - Defender.Defense;
                if (damage < 0)
                {
                    damage = 1;
                }
                Defender.Health -= damage;
                Console.WriteLine($"{Attacker.Name} does {damage} damage to {Defender.Name}");

                if (this.Missiles > 0)
                {
                    Console.WriteLine($"{Attacker.Name} fires a missile !");
                    damage = Atk;
                    Defender.Health -= damage;
                    Console.WriteLine($"{Attacker.Name} does {damage} damage to {Defender.Name} using the missile");
                }
            }
            public override int Calculate_attack()
            {
                Random random = new();
                int negative = random.Next(0, 1) * 2 - 1;
                int damage = Atk + (random.Next(0, this.Variance) * negative);
                if (damage < 0)
                {
                    damage = 1;
                }
                if (this.Missiles > 0)
                {
                    damage += Atk;
                }
                return damage;
            }
        }

        abstract class Unit

        {
            public Unit(string name)
            {
                this.Name = this.GetType().Name;
                this.Atk = 0;
                this.Defense = 0;
                this.Health = 0;
                this.Variance = 0;
                this.Speed = 0;
            }

            public int Atk { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }
            public string Name { get; set; }
            public int Speed { get; set; }
            public int Variance { get; set; }

            static public void Combat(Unit Defender, Unit Attacker)
            {
                while (Defender.Health > 0 && Attacker.Health > 0)
                {
                    Console.WriteLine($"{Attacker.Name} attacks {Defender.Name}");
                    Attacker.Attack(Defender, Attacker);
                    Console.WriteLine($"Defender has: {Defender.Health} health");
                    Console.WriteLine($"{Defender.Name} counterattacks {Attacker.Name}");
                    Defender.Attack(Attacker, Defender);
                    Console.WriteLine($"Defender has: {Attacker.Health} health");
                }
                if (Defender.Health <= 0)
                {
                    Console.WriteLine($"{Defender.Name} Loses.");
                }
                else
                {
                    Console.WriteLine($"{Attacker.Name} Loses.");
                }
            }

            public abstract void Attack(Unit Defender, Unit Attacker);
            public virtual int Calculate_attack() {
                return Atk;
            }
        }

        class Warrior : Unit
        {
            public Warrior(string name) : base(name)
            {
                this.Name = name;
                this.Atk = 5;
                this.Defense = 3;
                this.Health = 20;
                this.Variance = 2;
                this.Speed = 3;
            }

            public override void Attack(Unit Defender, Unit Attacker)
            {
                Random random = new();
                int negative = random.Next(0, 1) * 2 - 1;
                int damage = Atk + (random.Next(0, this.Variance) * negative) - Defender.Defense;
                if (damage < 0)
                {
                    damage = 1;
                }
                Defender.Health -= damage;
                Console.WriteLine($"{Attacker.Name} does {damage} damage to {Defender.Name}");
            }
            public override int Calculate_attack()
            {
                Random random = new();
                int negative = random.Next(0, 1) * 2 - 1;
                int damage = Atk + (random.Next(0, this.Variance) * negative);
                if (damage < 0)
                {
                    damage = 1;
                }
                return damage;
            }
        }
    }
}