using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace E_Liquid
{
    public class chem
    {
        public string name = "";//
        public float price = 0;//       €/ml
        public float fluidity = 0;//    drop/ml
        public float weight = 0;//      g/ml
        public float VG_conc = 0;//     %/100
        public float PG_conc = 0;//     %/100
        public float Wat_conc = 0;//    %/100
        public float nic = 0;//         mg/ml
        public chem(string name, float price, float fluidity, float weight, float VG_conc, 
                    float PG_conc, float Wat_conc, float nic=0)
        {
            this.name = name;
            this.price = price;
            this.fluidity = fluidity;
            this.weight = weight;
            float sum = VG_conc + PG_conc + Wat_conc;
            this.VG_conc = VG_conc / sum;
            this.PG_conc = PG_conc / sum;
            this.Wat_conc = Wat_conc / sum;
            this.nic = nic;
        }
    }

    static public class essential
    {
        public static chem Vg = new chem("vg", 0.0018f, 28.4f, 1.285f, 1, 0, 0);
        public static chem Pg = new chem("pg", 0.0018f, 46.39f, 1.060f, 0, 1, 0);
        public static chem Wat = new chem("water", 0.00001f, 50.0f, 1.0f, 0, 0, 1);
        public static chem Nic = new chem("nicotine", 0.01520f, 34.24f, 1.285f, 1, 0, 0, 100);
        public static chem Aroma = new chem("aroma", 0.50f, 40.0f, 1.060f, 0, 1, 0);
    }



    public class ingredients
    {
        public bool ok = true;
        public float pg = 0,       reqPG = 0;//        ml
        public float vg = 0,       reqVG = 0;//        ml
        public float wat = 0,      reqWat = 0;//       ml
        public float nic = 0,      reqNic = 0;//       ml
        public float pg_g = 0,     reqPG_g = 0;//      g
        public float vg_g = 0,     reqVG_g = 0;//      g
        public float wat_g = 0,    reqWat_g = 0;//     g
        public float nic_g = 0,    reqNic_g = 0;//     g
        public float p_pg = 0, p_vg = 0, p_wat = 0, p_nic = 0;//   €
        public float reqp_pg = 0, reqp_vg = 0, reqp_wat = 0, reqp_nic = 0;//   €
        public Dictionary<string, float> flav = new Dictionary<string, float>();//        ml
        public Dictionary<string, float> flav_g = new Dictionary<string, float>();//      g
        public Dictionary<string, float> reqFlav = new Dictionary<string, float>();//     ml
        public Dictionary<string, float> reqFlav_g = new Dictionary<string, float>();//   g
        public Dictionary<string, float> price = new Dictionary<string, float>();//       €
        public Dictionary<string, float> reqPrice = new Dictionary<string, float>();//       €

        public ingredients()  { }
        private float approx(float val, int dec = 4)
        {
            float sup = 0.5f;
            if (val < 0) sup = -sup;
            val = (float)(int)(sup + val * Math.Pow(10,dec)) / (float)Math.Pow(10, dec);
            return val;
        }
        public void check()
        {
            if (reqPG < 0 || reqVG < 0 || reqWat < 0 || reqNic < 0 || reqPG_g < 0 || reqVG_g < 0 || reqWat_g < 0) ok = false;
            foreach (string key in reqFlav.Keys) if (reqFlav[key] < 0) ok = false;
            foreach (string key in reqFlav_g.Keys) if (reqFlav_g[key] < 0) ok = false;

            pg = approx(pg);
            vg = approx(vg);
            wat = approx(wat);
            nic = approx(nic);
            pg_g = approx(pg_g);
            vg_g = approx(vg_g);
            wat_g = approx(wat_g);
            nic_g = approx(nic_g);
            reqPG = approx(reqPG);
            reqVG = approx(reqVG);
            reqWat = approx(reqWat);
            reqNic = approx(reqNic);
            reqPG_g = approx(reqPG_g);
            reqVG_g = approx(reqVG_g);
            reqWat_g = approx(reqWat_g);
            reqNic_g = approx(reqNic_g);
            foreach (string key in flav.Keys.ToArray()) flav[key] = approx(flav[key]);
            foreach (string key in flav_g.Keys.ToArray()) flav_g[key] = approx(flav_g[key]);
            foreach (string key in reqFlav.Keys.ToArray()) reqFlav[key] = approx(reqFlav[key]);
            foreach (string key in reqFlav_g.Keys.ToArray()) reqFlav_g[key] = approx(reqFlav_g[key]);

            p_vg = approx(vg * essential.Vg.price, 2);
            p_pg = approx(pg * essential.Pg.price, 2);
            p_wat = approx(wat * essential.Wat.price, 2);
            p_nic = approx(nic * essential.Nic.price, 2);
            foreach (string key in flav.Keys.ToArray())
                approx(price[key] = approx(flav[key]) * essential.Aroma.price, 2);

            reqp_vg = approx(reqVG * essential.Vg.price, 2);
            reqp_pg = approx(reqPG * essential.Pg.price, 2);
            reqp_wat = approx(reqWat * essential.Wat.price, 2);
            reqp_nic = approx(reqNic * essential.Nic.price, 2);
            foreach (string key in reqFlav.Keys.ToArray()) 
                reqPrice[key] = approx(reqFlav[key] * essential.Aroma.price, 2);

        }
        public ingredients copy()
        {
            ingredients output = new ingredients();
            output.vg = vg; output.pg = pg; output.wat = wat; output.vg_g = vg_g; 
            output.pg_g = pg_g; output.wat_g = wat_g; output.nic = nic;
            foreach (string k in flav.Keys) output.flav[k] = flav[k];
            foreach (string k in flav_g.Keys) output.flav_g[k] = flav_g[k];
            return output;
        }
        public void addVG(float q) { vg += q; vg_g += q * essential.Vg.weight; }
        public void addVG_g(float q) { vg_g += q; vg += q / essential.Vg.weight; }
        public void addPG(float q) { pg += q; pg_g += q * essential.Pg.weight; }
        public void addPG_g(float q) { pg_g += q; pg += q / essential.Pg.weight; }
        public void addWat(float q) { wat += q; wat_g += q * essential.Wat.weight; }
        public void addWat_g(float q) { wat_g += q; wat += q / essential.Wat.weight; }
        public void addNic(float q) { nic += q; nic_g += q * essential.Nic.weight; }
        public void addNic_g(float q) { nic_g += q; nic += q / essential.Nic.weight; }


        public void setReqVG(float q) { reqVG = q; reqVG_g = q * essential.Vg.weight; }
        public void setReqVG_g(float q) { reqVG_g = q; reqVG = q / essential.Vg.weight; }
        public void setReqPG(float q) { reqPG = q; reqPG_g = q * essential.Pg.weight; }
        public void setReqPG_g(float q) { reqPG_g = q; reqPG = q / essential.Pg.weight; }
        public void setReqWat(float q) { reqWat = q; reqWat_g = q * essential.Wat.weight; }
        public void setReqWat_g(float q) { reqWat_g = q; reqWat = q / essential.Wat.weight; }
        public void setReqNic(float q) { reqNic = q; reqNic_g = q * essential.Nic.weight; }
        public void setReqNic_g(float q) { reqNic_g = q; reqNic = q / essential.Nic.weight; }
        public void setReqFlav(string name, float q)
        { reqFlav[name] = q; reqFlav_g[name] = q * essential.Aroma.weight; }
        public void setReqFlav_g(string name, float q)
        { reqFlav_g[name] = q; reqFlav[name] = q / essential.Aroma.weight; }



        public void addFlav(string name, float q)
        {
            if (!flav.ContainsKey(name)) { flav[name] = 0; flav_g[name] = 0; }
            flav[name] += q; flav_g[name] += q * essential.Aroma.weight;
        }
        public void addFlav_g(string name, float q)
        {
            if (!flav.ContainsKey(name)) { flav[name] = 0; flav_g[name] = 0; }
            flav_g[name] += q; flav[name] += q / essential.Aroma.weight;
        }

        public ingredients add(ingredients other)
        {
            ingredients output = other.copy();
            float _vg = vg;
            float _pg = pg;
            float _wat = wat;
            output.addVG(_vg);
            output.addPG(_pg);
            output.addWat(_wat);
            output.addNic(nic);
            
            foreach(string arom in flav.Keys) output.addFlav(arom, flav[arom]);
            return output;
        }

        public float total() { return pg + vg + wat; }
        public float total_g() { return pg_g + vg_g + wat_g; }

        public void print(string unit, bool toAdd)
        {
            Console.WriteLine();
            if (unit == "ml")
            {
                Console.WriteLine("Objective:");
                Console.WriteLine("    VG:       " + vg + unit);
                Console.WriteLine("    PG:       " + pg + unit);
                Console.WriteLine("    Water:    " + wat + unit);
                Console.WriteLine("    Nicotine: " + nic + unit);
                foreach (string arom in flav.Keys) Console.WriteLine("    " + arom + ":         " + flav[arom] + unit);
                Console.WriteLine();

                if (toAdd)
                {
                    Console.WriteLine(" To Add:");
                    Console.WriteLine(" - Pure VG:           " + reqVG + unit);
                    Console.WriteLine(" - Pure PG:           " + reqPG + unit);
                    Console.WriteLine(" - Pure Water:        " + reqWat + unit);
                    Console.WriteLine(" - Diluited Nicotine: " + reqNic + unit);
                    foreach (string arom in reqFlav.Keys) 
                        Console.WriteLine(" - Diluited " + arom + ":         " + reqFlav[arom] + unit);
                    Console.WriteLine();
                }
            }
            else if (unit == "g")
            {
                Console.WriteLine("Objective:");
                Console.WriteLine("    VG:       " + vg_g + unit);
                Console.WriteLine("    PG:       " + pg_g + unit);
                Console.WriteLine("    Water:    " + wat_g + unit);
                Console.WriteLine("    Nicotine: " + nic_g + unit);
                foreach (string arom in flav_g.Keys) Console.WriteLine("    "+ arom + ":         " + flav_g[arom] + unit);
                Console.WriteLine();

                if (toAdd)
                {
                    Console.WriteLine(" To Add:");
                    Console.WriteLine(" - Pure VG:           " + reqVG_g + unit);
                    Console.WriteLine(" - Pure PG:           " + reqPG_g + unit);
                    Console.WriteLine(" - Pure Water:        " + reqWat_g + unit);
                    Console.WriteLine(" - Diluited Nicotine: " + reqNic_g + unit);
                    foreach (string arom in reqFlav_g.Keys) 
                        Console.WriteLine(" - Diluited " + arom + ":         " + reqFlav_g[arom] + unit);
                    Console.WriteLine();
                }
            }
        }
    }



    public class Kernel
    {
        public Dictionary<int, List<object>> buffer;
        public Dictionary<int, List<object>> buffer_out;
        public bool active = false;
        public Kernel()
        {
            Console.WriteLine("Kernel active!");
            buffer = new Dictionary<int, List<object>>();
            buffer_out = new Dictionary<int, List<object>>();
        }

        public void activity()
        {
            if(!active)
            {
                active = true;
                while (active)
                {
                    Thread.Sleep(50);
                    if (buffer.Count > 0)
                    {
                        int key = buffer.Keys.ToArray()[0];
                        var value = buffer[key];
                        buffer.Remove(key);
                        solve(key, value);
                    }
                }
            }
        }

        private void solve(int id, List<object> args)
        {
            if ((string)args[0] == "mix")
            {
                Console.WriteLine("Kernel is mixing");
                var prev = (ingredients)args[2];
                ingredients res = mix((string)args[1], prev, (float)args[3], (float)args[4],
                            (float)args[5], (float)args[6], (float)args[7], (Dictionary<string, float>)args[8]);
                buffer_out[id] = new List<object>() { res };
            }
            else if ((string)args[0] == "min")
            {
                Console.WriteLine("Kernel is about to find the minimum quantity to mix");
                var prev = (ingredients)args[2];
                float res = min_mix((string)args[1], prev, (float)args[4], (float)args[5],
                            (float)args[6], (float)args[7], (Dictionary<string, float>)args[8]);
                buffer_out[id] = new List<object>() { res };
            }
            else if ((string)args[0] == "add")
            {
                Console.WriteLine("Kernel is adding");
                var prev = (ingredients)args[2];
                var act = (ingredients)args[3];
                ingredients res = prev.add(act);
                buffer_out[id] = new List<object>() { res };
            }
        }


        private ingredients mix(string mode, ingredients prev, float total, float vg, float pg, float wat, float nic,
                                Dictionary<string, float> aroma)
        {
            ingredients tot;
            if (prev == null) tot = new ingredients();
            else tot = prev.copy();
            float sum = vg + pg + wat;
            vg /= sum; pg /= sum; wat /= sum;

            if (mode == "g")
            {
            }
            else if (mode == "ml")
            {
                //float prev_vg = tot.vg;
                //float prev_pg = tot.g;
                //float prev_wat = tot.wat;

                tot.setReqNic((nic / essential.Nic.nic) * total - tot.nic);
                tot.addVG(tot.reqNic * essential.Nic.VG_conc);
                tot.addPG(tot.reqNic * essential.Nic.PG_conc);
                tot.addWat(tot.reqNic * essential.Nic.Wat_conc);
                tot.addNic(tot.reqNic);

                foreach (string arom in tot.flav.Keys)
                {
                    if (!aroma.Keys.Contains(arom)) tot.setReqFlav(arom, -tot.flav[arom]);
                    //Console.WriteLine(arom);
                }
                foreach (string arom in aroma.Keys)
                {
                    float present = 0;
                    if (tot.flav.Keys.Contains(arom)) present = tot.flav[arom];
                    tot.setReqFlav(arom, aroma[arom] * total - present);
                    tot.addFlav(arom, tot.reqFlav[arom]);
                    tot.addVG(tot.reqFlav[arom] * essential.Aroma.VG_conc);
                    tot.addPG(tot.reqFlav[arom] * essential.Aroma.PG_conc);
                    tot.addWat(tot.reqFlav[arom] * essential.Aroma.Wat_conc);
                }

                tot.setReqVG((total * vg) - tot.vg);
                tot.setReqPG((total * pg) - tot.pg);
                tot.setReqWat((total * wat) - tot.wat);

                tot.addVG(tot.reqVG);
                tot.addPG(tot.reqPG);
                tot.addWat(tot.reqWat);

                tot.check();

                return tot;
            }
            return null;
        }
        private float min_mix(string mode, ingredients tot, float vg, float pg, float wat, float nic,
                                Dictionary<string, float> aroma)
        {
            float init = 0;
            float final = 10;
            ingredients res = null; int i;
            for (i = 0; i<29; i++)
            {
                res = mix(mode, tot, final, vg, pg, wat, nic, aroma);
                if (res.ok) break;
                init = final;
                final = (float)Math.Pow(10, i);
            }
            if (!res.ok)
            {
                Console.WriteLine("Max procedure did not converge (tried up to " + Math.Pow(10, i) + ").");
                return -1;
            }
            int tries = 1;
            while (final - init > 0.005 && tries < 200)
            {
                float x = (init + final) / 2;
                res = mix(mode, tot, x, vg, pg, wat, nic, aroma);
                if (res.ok) final = x; else init = x;
                tries += 1;
            }
            float min = (float)Convert.ToInt64(init * 100) / 100;
            return min;

        }
    }
}
