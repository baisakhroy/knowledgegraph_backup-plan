using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Neo4jClient;
using Neo4j.Driver.V1;

namespace csvtocypherinc_
{
    class Program
    {
        static void Main(string[] args)
        {
            int i,j;
            using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "abcdefg")))
using (var session = driver.Session())
            {
                Console.WriteLine("How many documnets are there to uplaod?");
                int n = int.Parse(Console.ReadLine());
                 
                System.Int64[] sum = new System.Int64[n];

                // int[,] sumdoc = new int[n,n];

                string[] query = new string[]{"single","Vinayak","girls"};

                string[] documentname = new string[n] ;

                
                
                for(i=0;i<n;i++){
                    Console.WriteLine("Enter the name of your document number {0}",(i+1));
                    documentname[i]= Console.ReadLine();
                        session.Run(@"load csv with headers from {filedirectory} as row FOREACH(ignoreMe IN CASE WHEN trim(row.tokens__partOfSpeech__tag) = {NOUN} THEN [1] ELSE [] END | MERGE (p:"+documentname[i]+" {name: row.tokens__text__content}))  FOREACH(ignoreMe IN CASE WHEN trim(row.tokens__partOfSpeech__tag) = {ADJ} THEN [1] ELSE [] END | MERGE (p:"+documentname[i]+" {name: row.tokens__text__content})) with row match(m:"+documentname[i]+"),(n:"+documentname[i]+") merge (m)-[:next]-(n)",
                 new Dictionary<string, object>{{"filedirectory","file:///"+documentname[i]+".csv"},{"NOUN","NOUN"},{"ADJ","ADJ"}});
                     
                    //  Console.WriteLine("{1} updated in neo4j graph",documentname[i]);
                    }
                

                
                 session.Run(@"match(a) match(b) where a.name=b.name merge (a)<-[r:same]->(b)");
                 session.Run(@"match(a) match(b) where a=b match (a)<-[r]->(b) delete r");

                 int querylength = query.Length;

                for(i=0;i<n;i++){                       //n represents the number of documents 
                    sum[i]=0;            
                    for(j=0;j<querylength;j++){              

                            


    var result = session.Run("MATCH (a:"+documentname[i]+")  where a.name = {name} " +
                             
                             "RETURN count(a) as count",
                             new Dictionary<string, object> {{"name",query[j]} });


            

    foreach (var record in result)
    {

       Console.WriteLine("{0}  in {1} is = {2}",query[j],documentname[i],$"{record["count"].As<int>()}");
        var COUNT = (record["count"]);
        sum[i] += (System.Int64)COUNT;
        

    }
    


                    }   
                    Console.WriteLine("Score of {0} is {1}",documentname[i],sum[i]);         
                    
                 }

        List<System.Int64> sumlist = sum.ToList();
            for(i=0;i<n;i++){
                Console.WriteLine("Maximum value is  "+sumlist.Max());
                Console.WriteLine("Return the file "+documentname[(sumlist.IndexOf(sumlist.Max()))]);
                sumlist[(sumlist.IndexOf(sumlist.Max()))] = -1 ;
                
            }
            

            }
        }
    }
}
