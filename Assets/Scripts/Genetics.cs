using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    private int mGeneLength;
    private int mNumGenes;
    private List<string> mGenes;
    /*
     32 bit genes = >
     source + 7 bit id + sink + 7 bit id + 16 bit weight
     7 bit ids allows 128 neuron types
    */
    public Genome(int geneLength = 32, int numGenes = 8, bool genesis = false)
    {
        GeneLength = geneLength;
        NumGenes = numGenes;
    }

    public int GeneLength { get => mGeneLength; set => mGeneLength = value; }
    public int NumGenes { get => mNumGenes; set => mNumGenes = value; }
    public List<string> Genes { get => mGenes; set => mGenes = value; }

    public void Randomize()
    {
        Genes = new List<string>();
        for(int k=0;k<NumGenes;k++)
        {
            string thisGene = "";
            for(int i=0;i<GeneLength;i++)
                thisGene += Mathf.Round(Random.value).ToString();
            Genes.Add(thisGene);
        }
    }

    public Color Color()
    {
        float r=0,g=0,b=0;

        for(int j=0;j<3;j++)
        {
            string thisGene = Genes[j];
            
            r += System.Convert.ToInt32(thisGene.Substring(0,8),2);
            g += System.Convert.ToInt32(thisGene.Substring(8,8),2);
            b += System.Convert.ToInt32(thisGene.Substring(16,8),2);
            
        }
        return new Color(r/2f/255f,g/2f/255f,b/2f/255f);
    }

    public (int, int) Source(string gene)
    {
        int type = System.Convert.ToInt32(gene.Substring(0,1),2);
        int id = System.Convert.ToInt32(gene.Substring(1,7),2);
        return (type, id);
    }

    public (int, int) Sink(string gene)
    {
        int type = System.Convert.ToInt32(gene.Substring(8,1),2);
        int id = System.Convert.ToInt32(gene.Substring(9,7),2);
        return (type, id);
    }

    public float Strength(string gene)
    {
        //System.Convert.ToDouble(gene.Substring(16,16),)/8000f;
        long i = System.Convert.ToInt64(gene.Substring(16,16),2);
        return (float)i/8000f-4f;
    }

    public void Mutate(int n)
    {
        for(int k=0;k<n;k++)
        {
            int g = Random.Range(0,NumGenes);
            int b = Random.Range(0,GeneLength);
            string gene = Genes[g];
            int newBit = (int)(1-System.Convert.ToInt32(gene.Substring(b,1),2));
            string newGene = "";
            for(int i=0;i<GeneLength;i++)
                newGene += (i==b ? newBit.ToString() : gene.Substring(i,1));
            Genes[g] = newGene;
        }
    }
        
}
