using System;
using System.Collections.Generic;

abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }
    private string lastSkillUsed = "";

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public virtual void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage <= 0)
        {
            damage = 1;
        }
        target.Energi -= damage;
        Energi -= 10;
        Console.WriteLine($"{Nama} menyerang {target.Nama}, menyebabkan {damage} damage! Energi {Nama} sekarang {Energi}.");
    }

    public void GunakanKemampuan(IKemampuan kemampuan)
    {
        if (lastSkillUsed != kemampuan.NamaKemampuan())
        {
            if (Energi > 20) 
            {
                kemampuan.Gunakan(this);
                Energi -= 20;
                lastSkillUsed = kemampuan.NamaKemampuan();
            }
            else
            {
                Console.WriteLine($"{Nama} tidak memiliki cukup energi untuk menggunakan {kemampuan.NamaKemampuan()}.");
            }
        }
        else
        {
            Console.WriteLine($"Kemampuan {kemampuan.NamaKemampuan()} sedang cooldown. Pilih kemampuan lain.");
        }
    }

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

interface IKemampuan
{
    string NamaKemampuan();
    void Gunakan(Robot robot);
}

class Repair : IKemampuan
{
    public string NamaKemampuan() => "Repair";

    public void Gunakan(Robot robot)
    {
        robot.Energi += 50;
        Console.WriteLine($"{robot.Nama} menggunakan Repair, energi dipulihkan 50!");
    }
}

class ElectricShock : IKemampuan
{
    public string NamaKemampuan() => "Electric Shock";

    public void Gunakan(Robot robot)
    {
        Console.WriteLine($"{robot.Nama} menggunakan Electric Shock, menyerang dengan efek listrik!");
    }
}

class PlasmaCannon : IKemampuan
{
    public string NamaKemampuan() => "Plasma Cannon";

    public void Gunakan(Robot robot)
    {
        Console.WriteLine($"{robot.Nama} menggunakan Plasma Cannon, serangan menembus armor!");
    }
}

class SuperShield : IKemampuan
{
    public string NamaKemampuan() => "Super Shield";

    public void Gunakan(Robot robot)
    {
        robot.Armor += 20;
        Console.WriteLine($"{robot.Nama} menggunakan Super Shield, armor meningkat 20!");
    }
}

class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - Armor;
        if (damage <= 0)
        {
            damage = 1;
        }
        Energi -= damage;
        Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama}, menyebabkan {damage} damage! Energi {Nama} sekarang {Energi}.");
    }

    public void Mati()
    {
        if (Energi <= 0)
        {
            Console.WriteLine($"{Nama} telah mati!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Selamat Datang Di Pertarungan Robot!");
        Console.Write("Masukkan nama pemain: ");
        string namaPemain = Console.ReadLine();

        // Membuat robot pemain dan bos robot
        Robot robotPemain = new BosRobot(namaPemain, 200, 40, 30);
        Robot bos = new BosRobot("BossBot", 300, 50, 40);

        // Membuat kemampuan yang bisa dipilih
        List<IKemampuan> kemampuan = new List<IKemampuan>
        {
            new Repair(),
            new ElectricShock(),
            new PlasmaCannon(),
            new SuperShield()
        };

        bool gameBerjalan = true;

        while (gameBerjalan)
        {
            Console.WriteLine("\nPilih kemampuan untuk digunakan:");
            for (int i = 0; i < kemampuan.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {kemampuan[i].NamaKemampuan()}");
            }

            int pilihan;
            if (int.TryParse(Console.ReadLine(), out pilihan) && pilihan >= 1 && pilihan <= kemampuan.Count)
            {
                robotPemain.GunakanKemampuan(kemampuan[pilihan - 1]);
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid.");
            }

            // Bos menyerang balik pemain
            bos.Serang(robotPemain);
            robotPemain.CetakInformasi();
            bos.CetakInformasi();

            if (robotPemain.Energi <= 0)
            {
                Console.WriteLine("Pemain telah kalah.");
                gameBerjalan = false;
            }
            else if (bos.Energi <= 0)
            {
                Console.WriteLine("Bos telah kalah.");
                gameBerjalan = false;
            }
        }
    }
}
