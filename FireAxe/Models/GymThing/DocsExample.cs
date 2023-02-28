﻿namespace FireAxe.Models.GymThing
{
    internal class DocsExample
    {
        private static Random rnd = new Random(1);

        private static void Main(string[] args)
        {
            Console.WriteLine("Begin Q-learning maze demo");
            Console.WriteLine("Setting up maze and rewards");
            int ns = 12;
            int[][] FT = CreateMaze(ns);
            float[][] R = CreateReward(ns);
            float[][] Q = CreateQuality(ns);
            Console.WriteLine("Analyzing maze using Q-learning");
            int goal = 11;
            float gamma = 0.5f;
            float learnRate = 0.5f;
            int maxEpochs = 1000;
            Train(FT, R, Q, goal, gamma, learnRate, maxEpochs);
            Console.WriteLine("Done. Q matrix: ");
            Print(Q);
            Console.WriteLine("Using Q to walk from cell 8 to 11");
            Walk(8, 11, Q);
            Console.WriteLine("End demo");
            Console.ReadLine();
        }

        private static void Print(float[][] Q)
        {
            int ns = Q.Length;
            Console.WriteLine("[0] [1] . . [11]");
            for (int i = 0; i < ns; ++i)
            {
                for (int j = 0; j < ns; ++j)
                {
                    Console.Write(Q[i][j].ToString("F2") + " ");
                }
                Console.WriteLine();
            }
        }

        private static int[][] CreateMaze(int ns)
        {
            int[][] FT = new int[ns][];
            for (int i = 0; i < ns; ++i) FT[i] = new int[ns];
            FT[0][1] = FT[0][4] = FT[1][0] = FT[1][5] = FT[2][3] = 1;
            FT[2][6] = FT[3][2] = FT[3][7] = FT[4][0] = FT[4][8] = 1;
            FT[5][1] = FT[5][6] = FT[5][9] = FT[6][2] = FT[6][5] = 1;
            FT[6][7] = FT[7][3] = FT[7][6] = FT[7][11] = FT[8][4] = 1;
            FT[8][9] = FT[9][5] = FT[9][8] = FT[9][10] = FT[10][9] = 1;
            FT[11][11] = 1;  // Goal
            return FT;
        }

        private static float[][] CreateReward(int ns)
        {
            float[][] R = new float[ns][];
            for (int i = 0; i < ns; ++i) R[i] = new float[ns];
            R[0][1] = R[0][4] = R[1][0] = R[1][5] = R[2][3] = -0.1f;
            R[2][6] = R[3][2] = R[3][7] = R[4][0] = R[4][8] = -0.1f;
            R[5][1] = R[5][6] = R[5][9] = R[6][2] = R[6][5] = -0.1f;
            R[6][7] = R[7][3] = R[7][6] = R[7][11] = R[8][4] = -0.1f;
            R[8][9] = R[9][5] = R[9][8] = R[9][10] = R[10][9] = -0.1f;
            R[7][11] = 10.0f;  // Goal
            return R;
        }

        private static float[][] CreateQuality(int ns)
        {
            float[][] Q = new float[ns][];
            for (int i = 0; i < ns; ++i)
                Q[i] = new float[ns];
            return Q;
        }

        private static List<int> GetPossNextStates(int s, int[][] FT)
        {
            List<int> result = new List<int>();
            for (int j = 0; j < FT.Length; ++j)
                if (FT[s][j] == 1) result.Add(j);
            return result;
        }

        private static int GetRandNextState(int s, int[][] FT)
        {
            List<int> possNextStates = GetPossNextStates(s, FT);
            int ct = possNextStates.Count;
            int idx = rnd.Next(0, ct);
            return possNextStates[idx];
        }

        private static void Train(int[][] FT, float[][] R, float[][] Q,
             int goal, float gamma, float lrnRate, int maxEpochs)
        {
            for (int epoch = 0; epoch < maxEpochs; ++epoch)
            {
                int currState = rnd.Next(0, R.Length);
                while (true)
                {
                    int nextState = GetRandNextState(currState, FT);
                    List<int> possNextNextStates = GetPossNextStates(nextState, FT);
                    float maxQ = float.MinValue;
                    for (int j = 0; j < possNextNextStates.Count; ++j)
                    {
                        int nns = possNextNextStates[j];  // short alias
                        float q = Q[nextState][nns];
                        if (q > maxQ) maxQ = q;
                    }
                    Q[currState][nextState] =
                        ((1 - lrnRate) * Q[currState][nextState]) +
                        (lrnRate * (R[currState][nextState] + (gamma * maxQ)));

                    currState = nextState;
                    if (currState == goal) break;
                }
            }

        }

        private static void Walk(int start, int goal, float[][] Q)
        {
            int curr = start; int next;
            Console.Write(curr + "->");
            while (curr != goal)
            {
                next = ArgMax(Q[curr]);
                Console.Write(next + "->");
                curr = next;
            }
            Console.WriteLine("done");
        }


        private static int ArgMax(float[] vector)
        {
            float maxVal = vector[0]; int idx = 0;
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > maxVal)
                {
                    maxVal = vector[i]; idx = i;
                }
            }
            return idx;
        }
    }
} //

