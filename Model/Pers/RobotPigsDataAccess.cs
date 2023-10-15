
namespace RobotPigs.Persistence
{
    public class RobotPigsDataAccess : IRobotPigsDataAccess
    {
        /// <summary>
        ///  Load a file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>The loaded Board.</returns>
        public async Task<Board> LoadAsync(String path)
        {
            try
            {
                using StreamReader reader = new(path); // fájl megnyitása
                String line = await reader.ReadLineAsync() ?? String.Empty;
                String[] numbers;
                Int32 boardSize = System.Int32.Parse(line);


                Pos[] pos = new Pos[2];
                int[] hps = new int[2];
                int[] buff = new int[4];
                for (int i = 0; i < 2; i++)
                {
                    line = await reader.ReadLineAsync() ?? String.Empty;
                    numbers = line.Split(' ');
                    for (int j = 0; j < 4; j++)
                    {
                        buff[j] = System.Int32.Parse(numbers[j]);
                    }
                    pos[i] =
                        new Persistence.Pos(buff[0], buff[1], (Persistence.Pos.Direction)buff[2]);
                    hps[i] = buff[3];
                }

                return new Board(boardSize, (pos[0], hps[0]), (pos[1], hps[1]));
            }
            catch (FileNotFoundException ex)
            {
                throw new BoardDataException("Could not find file.", ex);
            }
            catch (IOException ex)
            {
                throw new BoardDataException("There was an error reading from file", ex);
            }
            catch (Exception ex)
            {
                throw new BoardDataException("Exception",ex);
            }
        }

        /// <summary>
        /// Save into a file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="board">The board to save..</param>
        public async Task SaveAsync(String path, Board board)
        {
            try
            {
                using StreamWriter writer = new(path); // fájl megnyitása
                await writer.WriteLineAsync(Convert.ToString(board.N));

                await writer.WriteAsync(board.Plr1.Pos.X + " ");
                await writer.WriteAsync(board.Plr1.Pos.Y + " ");
                await writer.WriteAsync((int)board.Plr1.Pos.Dir + " ");
                await writer.WriteAsync(board.Plr1.Hp + " ");
                await writer.WriteLineAsync();

                await writer.WriteAsync(board.Plr2.Pos.X + " ");
                await writer.WriteAsync(board.Plr2.Pos.Y + " ");
                await writer.WriteAsync((int)board.Plr2.Pos.Dir + " ");
                await writer.WriteAsync(board.Plr2.Hp + " ");
                await writer.WriteLineAsync();
            }
            catch (FileNotFoundException ex)
            {
                throw new BoardDataException("Could not find file.", ex);
            }
            catch (IOException ex)
            {
                throw new BoardDataException("There was an error reading from file", ex);
            }
            catch (Exception ex)
            {
                throw new BoardDataException("Exception", ex);
            }
        }
    }
}