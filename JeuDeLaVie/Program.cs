using System;
using System.Collections.Generic;
using System.Text;

namespace JeuDeLaVie {
	/**
	 * Cette classe contient des méthodes pouvant simplfier la communication avec l'utilisateur, en particulier en
	 * s'assurant que ses erreurs ne causent pas de problème à l'exécution du programme.
	 */
	static class CommunicationUtilities {
		/**
		 * Cette méthode assure une entrée correcte de la part de l'utilisateur. Le message est affiché, puis, tant
		 * qu'il répond mal, une réponse valide lui est demandée. Elle retourne un bool.
		 * @param string Le message à afficher pour demander un input à l'utilisateur.
		 * @return La valeur fournie par l'utilisateur en réponse à la demande.
		 */
		public static bool GetBoolInput(string message) {
			Console.Write(message);

			while (true) {
				var c = Console.ReadKey(false).KeyChar;
				Console.WriteLine();
				if (c == 'y' || c == 'Y') return true;
				if (c == 'n' || c == 'N') return false;
				Console.Write("Veuillez entrer y, Y, n ou N. Réessayez : ");
			}
		}

		/**
		 * Cette méthode assure une entrée correcte de la part de l'utilisateur. Le message est affiché, puis, tant
		 * qu'il répond mal, une réponse lui est demandée. Elle retourne un int signé.
		 * @param string Le message à afficher pour demander un input à l'utilisateur.
		 * @return La valeur fournie par l'utilisateur en réponse à la demande.
		 */
		public static int GetIntInput(string message) {
			Console.Write(message);

			var result = 0;
			var flag = true;
			do {
				try {
					flag = true;
					result = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception) {
					Console.Write("Invalide. Réessayez : ");
					flag = false;
				}
			} while (!flag);

			return result;
		}

		/**
		 * Cette méthode assure une entrée correcte de la part de l'utilisateur. Le message est affiché, puis, tant
		 * qu'il répond mal, une réponse lui est demandée. Elle retourne un int non signé.
		 * @param string Le message à afficher pour demander un input à l'utilisateur.
		 * @return La valeur fournie par l'utilisateur en réponse à la demande.
		 */
		public static uint GetUintInput(string message) {
			Console.Write(message);

			var result = 0u;
			var flag = true;
			do {
				try {
					flag = true;
					result = Convert.ToUInt32(Console.ReadLine());
				}
				catch (Exception) {
					Console.Write("Invalide. Réessayez : ");
					flag = false;
				}
			} while (!flag);

			return result;
		}

		/**
		 * Cette méthode assure une entrée correcte de la part de l'utilisateur. Le message est affiché, puis, tant
		 * qu'il répond mal, une réponse lui est demandée. Elle retourne un float.
		 * @param string Le message à afficher pour demander un input à l'utilisateur.
		 * @return La valeur fournie par l'utilisateur en réponse à la demande.
		 */
		public static double GetFloatInput(string message) {
			Console.Write(message);

			var result = 0.0;
			var flag = true;
			do {
				try {
					flag = true;
					result = Convert.ToDouble(Console.ReadLine());
				}
				catch (Exception) {
					Console.Write("Invalide. Réessayez : ");
					flag = false;
				}
			} while (!flag);

			return result;
		}
	}

	/**
	 * Cette classe simplifie l'utilisation du Random standard. Une instance reçoit un ratio à sa construction et
	 * l'utilise pour générer un booléen pondéré. 
	 * @brief Générateur de booléen aléatoire à base de ratio.
	 */
	class WeightedRandomGenerator {
		private readonly Random m_random;
		private readonly float m_ratio;

		/**
		 * Seul constructeur de la classe, il initialise l'objet avec le ratio passée en argument.
		 * @param ratio Le ratio utilisé pour la génération aléatoire.
		 * @throws ArgumentOutOfRangeException si le ratio n'est pas compris entre 0.1f et 0.9f.
		 */
		public WeightedRandomGenerator(float ratio) {
			m_random = new Random();

			if (0.1f > ratio || ratio > 0.9f)
				throw new ArgumentOutOfRangeException(nameof(ratio),
					"Le ratio doit être compris entre 0.1f et 0.9f.");
			m_ratio = ratio;
		}

		/**
		 * Cette méthode génère un nombre aléatoire floattant et le compare au ratio. De cette manière, la méthode
		 * retourne true à la fréquence exprimée par le ratio fourni à la construction.
		 * @return True ou false, d'une manière aléatoire poindérée par le ratio.
		 */
		public bool GenerateWeightedBool() {
			return m_random.NextDouble() <= m_ratio;
		}
	}

	/**
	 * Cette classe représente une cellule. Elle est soit morte soit vivante, et représente cet état par un bool.
	 * @brief Objet cellule utilisé par le client.
	 */
	class Cell {
		private bool m_state;

		/**
		 * Ce constructeur initialise la cellule avec l'état fourni en argument.
		 * @param living L'état initial de la cellule.
		 */
		public Cell(bool living) {
			m_state = living;
		}

		/**
		 * Cette méthode change l'état de la cellule pour celui passé en argument. L'état fourni peut être le même que
		 * celui de la cellule.
		 * @param newState Le nouvel état de la cellule, true pour vivant et false pour mort.
		 * @return True si la cellule a effectivement changé d'état, false dans le cas contraire.
		 */
		public bool SetState(bool newState) {
			if (newState == m_state) return false;
			m_state = newState;
			return true;
		}

		/**
		 * Cette méthode retourne le caractère qui représente au mieux la cellule.
		 * @return '#' si la cellule est vivante et ' ' si la cellule est morte.
		 */
		public char GetDrawnChar() {
			return m_state ? '#' : ' ';
		}
	}

	/**
	 * La classe CellPop représente une cellule appartenant à une population. Elle étend par composition la classe Cell,
	 * en contenant une instance de Cell et un id.
	 * @brief Cellule appartenant à une population.
	 */
	class CellPop {
		private readonly Cell m_cell;
		private int m_id;

		/**
		 * Ce constructeur crée la cellule en donnant à l'instance de Cell stockée l'état living et en stockant l'id.
		 * @param living L'état initial de la cellule interne
		 * @param id La population à laquelle cette cellule appartient.
		 */
		public CellPop(bool living, int id) {
			m_cell = new Cell(living);
			m_id = id;
		}

		/**
		 * Cette méthode change l'état et la population de la cellule pour ceux passés en argument. L'état et la
		 * population fournis peuvent être les mêmes que ceux passés en argument.
		 * @param newState Le nouvel état de la cellule, true pour vivant et false pour mort.
		 * @param newId La nouvelle population à laquelle la cellule appartient.
		 * @return True si l'état ou la population a(ont) changé(s), false sinon.
		 */
		public bool SetState(bool newState, int newId) {
			if (m_id == newId) return m_cell.SetState(newState);
			m_id = newId;
			m_cell.SetState(newState);
			return true;
		}

		/**
		 * Fournit la population à laquelle cette cellule appartient.
		 * @return Le int fourni à cette cellule à sa construction ou à son dernier changement d'état.
		 */
		public int GetPopulationId() {
			return m_id;
		}

		/**
		 * Cette méthode retourne le caractère qui représente au mieux la cellule.
		 * @return '#' si la cellule est vivante et '-' si la cellule est morte.
		 */
		public char GetDrawnChar() {
			return m_cell.GetDrawnChar();
		}

		/**
		 * Cette méthode retourne la couleur associée à la population de la cellule.
		 * @return Une couleur de console.
		 */
		public ConsoleColor GetConsoleColor() {
			switch (m_id % 14) {
				case 0: return ConsoleColor.Black;
				case 1: return ConsoleColor.DarkBlue;
				case 2: return ConsoleColor.DarkGreen;
				case 3: return ConsoleColor.DarkCyan;
				case 4: return ConsoleColor.DarkRed;
				case 5: return ConsoleColor.DarkMagenta;
				case 6: return ConsoleColor.DarkYellow;
				case 7: return ConsoleColor.Gray;
				case 8: return ConsoleColor.DarkGray;
				case 9: return ConsoleColor.Blue;
				case 10: return ConsoleColor.Green;
				case 11: return ConsoleColor.Cyan;
				case 12: return ConsoleColor.Red;
				case 13: return ConsoleColor.Magenta;
				case 14: return ConsoleColor.Yellow;
				default: return ConsoleColor.White;
			}
		}
	}

	/**
	 * Cette structure représente une matrice de type quelconque dont chaque côté est lié au côté lui faisant face. Sa
	 * taille est donc nécessairement d'au moins 2 dans chaque dimension. Elle n'est pas conçue comme wrapper safe d'une
	 * matrice built-in, mais plutôt comme une extension optionnelle de ses fonctionnalités.
	 * @brief Matrice virtuellement bouclée.
	 * @tparam T Le type des éléments stockés dans la matrice.
	 */
	struct LoopingMatrix<T> {
		private readonly T[,] m_matrix;

		/**
		 * Ce constructeur alloue la mémoire nécessaire à la matrice. Il vérifie que les valeurs passées sont
		 * acceptables.
		 * @param width Largeur de la matrice à créer.
		 * @param height Hauteur de la matrice à créer.
		 * @throws ArgumentOutOfRangeException si au moins l'une des deux dimensions est inférieure à 2.
		 */
		public LoopingMatrix(int width, int height) {
			if (width < 3)
				throw new ArgumentOutOfRangeException(nameof(width),
					"La largeur de la matrice doit être d'au moins 3.");
			if (height < 3)
				throw new ArgumentOutOfRangeException(nameof(height),
					"La hauteur de la matrice doit être d'au moins 3.");
			m_matrix = new T[height, width];
		}

		/**
		 * Ce getter retourne la matrice interne et permet un accès direct, par exemple pour lire tous les éléments
		 * sans utiliser la fonctionnalité de bouclage des coordonnées. Cette accès est conçu comme une optimisation
		 * quand le client peut s'assurer qu'il n'utilisera pas de coordonnées en dehors de la matrice.
		 */
		public T[,] InternalMatrix => m_matrix;

		/**
		 * Cette méthode permet un accès à la valeur stockée dans la matrice à une position virtuelle quelconque. Les
		 * coordonnées en entrée sont bouclées à l'aide de l'opérateur modulo.
		 * @param i La coordonnée i dans la matrice, dans toutes les valeurs représentables par un int.
		 * @param j La coordonnée j dans la matrice, dans toutes les valeurs représentables par un int.
		 * @return Une référence à la variable stockée en [i % width, j % height] dans la matrice.
		 */
		public ref T LoopingGet(int i, int j) {
			i = (i % m_matrix.GetLength(0) + m_matrix.GetLength(0)) % m_matrix.GetLength(0);
			j = (j % m_matrix.GetLength(1) + m_matrix.GetLength(1)) % m_matrix.GetLength(1);

			return ref m_matrix[i, j];
		}
	}

	/**
	 * Cette interface représente un objet qui applique un algorithme quelconque aux coordonées que le contexte lui
	 * fournit. Il est Implementor dans le design pattern Bridge, et ICellAccessor est l'Abstraction. Il est guidé dans
	 * la matrice par ce dernier. Cette structure permet de découpler la logique de modification, qui dépend de la
	 * nature des objets stockés dans la matrice, de la logique de déplacement dans la matrice, qui n'en dépend pas.
	 * Cette abstraction représente la logique de modification.
	 * @brief Algorithme applicable à plusieurs cases.
	 */
	interface ICellModifier {
		/**
		 * Cette méthode applique l'algorithme aux coordonnées fournies.
		 * @param i Coordonnée i dans la matrice.
		 * @param j Coordonnée j dans la matrice.
		 */
		void Access(int i, int j);
	}

	/**
	 * Cette classe est une implémentation partielle de l'interface ICellModifier. Elle simplifie la création des
	 * classes concrètes en écrivant de manière générique un constructeur qui accepte une matrice de types quelconques.
	 * @brief Implémentation partielle générique de ICellModifier.
	 */
	abstract class CellModifier<T> : ICellModifier {
		protected readonly LoopingMatrix<T> m_matrix;

		/**
		 * Ce constructeur stocke la matrice fournie pour son utilisation future dans la méthode Access.
		 * @param matrix La matrice sur laquelle appliquer l'algorithme.
		 */
		protected CellModifier(LoopingMatrix<T> matrix) {
			m_matrix = matrix;
		}

		/**
		 * Cette méthode applique l'algorithme aux coordonnées fournies.
		 * @param i Coordonnée i dans la matrice.
		 * @param j Coordonnée j dans la matrice.
		 */
		public abstract void Access(int i, int j);
	}

	/**
	 * Cette classe implémente l'interface ICellModifier. Elle est capable d'appliquer un delta aux compteurs de
	 * cellules vivantes dans la matrice fournie à sa construction. Le choix des cases est réalisé par l'Implementor
	 * auquel elle est associée.
	 * @brief Delta pour matrice de cellules à une population.
	 */
	class OnePopDelta : CellModifier<Tuple<Cell, short>> {
		private readonly short m_delta;

		/**
		 * Ce constructeur passe la matrice au constructeur de l'implémentation partielle CellModifier<T>. Il stocke
		 * également le delta pour l'appliquer aux cases voulues.
		 * @param matrix La matrice dans laquelle modifier les compteurs
		 * @param delta Le delta à appliquer aux compteurs
		 */
		public OnePopDelta(LoopingMatrix<Tuple<Cell, short>> matrix, short delta) : base(matrix) {
			m_delta = delta;
		}

		/**
		 * Modifie les compteurs à la coordonnée [i, j] en leur appliquant le delta fourni à la construction de l'objet.
		 * @param i La coordonnée i du compteur à modifier.
		 * @param j La coordonnée j du compteur à modifier.
		 */
		public override void Access(int i, int j) {
			ref var tuple = ref m_matrix.LoopingGet(i, j);
			tuple = new Tuple<Cell, short>(tuple.Item1, (short) (tuple.Item2 + m_delta));
		}
	}

	/**
	 * Cette classe implémente l'interface ICellModifier. Elle est capable d'appliquer un delta aux compteurs de
	 * cellules vivantes de la population indiquée dans la matrice fournie à la construction. Le choix des cases est
	 * réalisé par l'Implementor auquel elle est associée.
	 * @brief Delta pour matrice de cellules à une population.
	 */
	class MultiPopDelta : CellModifier<Tuple<CellPop, short[]>> {
		private readonly short m_delta;
		private readonly int m_population;

		/**
		 * Ce constructeur passe la matrice au constructeur de l'implémentation partielle CellModifier<T>. Il stocke
		 * également le delta à appliquer et la population à laquelle est liée le delta.
		 * @param matrix La matrice dans laquelle modifier les compteurs
		 * @param delta Le delta à appliquer aux compteurs
		 * @param population La population concernée par le delta et dont le compteur sera mis à jour.4
		 */
		public MultiPopDelta(LoopingMatrix<Tuple<CellPop, short[]>> matrix, short delta, int population)
			: base(matrix) {
			m_delta = delta;
			m_population = population;
		}

		/**
		 * Modifie le compteur relatif à la population m_population à la coordonnée [i, j] en lui appliquant le delta
		 * fourni à la construction de l'objet.
		 * @param i La coordonnée i du compteur à modifier.
		 * @param j La coordonnée j du compteur à modifier.
		 */
		public override void Access(int i, int j) {
			m_matrix.LoopingGet(i, j).Item2[m_population] += m_delta;
		}
	}

	/**
	 * Cette classe représente un algorithme à appliquer à une certaine zone de la matrice. L'algorithme lui-même est
	 * réalisé par l'Implementor fourni à la construction, et la responsabilité de la sélection de la zone où
	 * l'appliquer est laissée à cette classe. Cette classe est l'Abstraction dans le cadre du Bridge pattern, là où
	 * ICellModifier est l'Implementor.
	 * @brief Algorithme applicable sur une zone.
	 */
	abstract class IMatrixModifier {
		protected readonly ICellModifier m_implementor;

		/**
		 * Ce constructeur stocke l'Implementor pour l'utiliser lors de l'application de l'algorithme.
		 * @param implementor L'objet qui implémente l'algorithme de modification de la matrice.
		 */
		protected IMatrixModifier(ICellModifier implementor) {
			m_implementor = implementor;
		}

		/**
		 * Cette méthode applique la modification réalisée par l'Implementor à plusieurs cases dans la matrice définies
		 * par cette objet.
		 */
		public abstract void ApplyModification();
	}

	/**
	 * Cette classe représente un algorithme à appliquer autour d'une cellule. La coordonnée de la cellule centrale est
	 * fournie à la construction, et l'itération est de la responsibilité de cette classe. L'algorithme à appliquer est
	 * représenté par l'ICellModifier fourni en construction.
	 * @brief Modificateur des cases autour d'une coordonnée.
	 */
	class CirclingModifier : IMatrixModifier {
		private readonly int m_i;
		private readonly int m_j;

		/**
		 * Ce constructeur fournit l'implementor à son abstraction de base IMatrixModifier, et stocke les coordonnée de
		 * la case autour de laquelle appliquer la modification.
		 */
		public CirclingModifier(ICellModifier implementor, int i, int j) : base(implementor) {
			m_i = i;
			m_j = j;
		}

		/**
		 * Cette méthode applique l'algorithme de l'Implementor ICellModifier aux cases autour de [i, j].
		 */
		public override void ApplyModification() {
			for (int i = m_i - 1; i < m_i + 2; i++) {
				for (int j = m_j - 1; j < m_j + 2; j++) {
					if (i == m_i && j == m_j) continue;
					m_implementor.Access(i, j);
				}
			}
		}
	}

	/**
	 * Cette classe stocke l'état de la matrice au moment de sa construction, pour comparaison future (cf pattern
	 * Memento).
	 * @brief Memento de la matrice
	 */
	class GridMemento : IEquatable<GridMemento> {
		private readonly int[] m_cellTab;

		/**
		 * Ce constructeur crée le memento à partir d'une matrice multi-population en copiant son contenu dans un
		 * tableau.
		 * @param matrix La matrice à stocker
		 */
		public GridMemento(LoopingMatrix<Tuple<CellPop, short[]>> matrix) {
			m_cellTab = new int[matrix.InternalMatrix.Length];
			for (int i = 0; i < matrix.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < matrix.InternalMatrix.GetLength(1); j++) {
					m_cellTab[j + i * matrix.InternalMatrix.GetLength(0)] =
						matrix.InternalMatrix[i, j].Item1.GetPopulationId();
				}
			}
		}

		/**
		 * Ce constructeur crée le memento à partir d'une matrice uni-population en copiant son contenu dans un tableau.
		 * @param matrix La matrice à stocker
		 */
		public GridMemento(LoopingMatrix<Tuple<Cell, short>> matrix) {
			m_cellTab = new int[matrix.InternalMatrix.Length];
			for (int i = 0; i < matrix.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < matrix.InternalMatrix.GetLength(1); j++) {
					m_cellTab[j + i * matrix.InternalMatrix.GetLength(0)] =
						matrix.InternalMatrix[i, j].Item1.GetDrawnChar() == ' ' ? 0 : 1;
				}
			}
		}

		/**
		 * Cette méthode compare l'autre memento avec celui, en quittant la fonction dès qu'une différence est trouvée.
		 * @return True si l'autre memento stocke le même contenu.
		 */
		public bool Equals(GridMemento other) {
			for (int i = 0; i < m_cellTab.Length; i++) {
				if (m_cellTab[i] != other.m_cellTab[i]) return false;
			}

			return true;
		}
	}

	/**
	 * Cette interface définit les fonctionnalités nécessaires qu'une grille de cellules doit posséder. Elle doit
	 * pouvoir être mise à jour, sérialisée et affichée.
	 * @brief Interface d'une grille.
	 */
	interface IGrid {
		/**
		 * Cette méthode met à jour la grille, pour la faire avancer d'une population.
		 */
		void Update();

		/**
		 * Cette méthode retourne un objet qui stocke l'état interne de la grille, et dont les instances peuvent être
		 * comparées.
		 * @return Une instance de GridMemento rendant compte de l'état de la grille au moment de l'exécution de la
		 * méthode.
		 */
		GridMemento Serialize();

		/**
		 * Cette méthode affiche la grille.
		 */
		void Draw();
	}

	/**
	 * Cette classe représente une grille de cellules appartenant toute à la même population. Cette classe est une
	 * optimisation dans la mesure où MultiPopGrid supporte une unique population, mais que son algorithme de mise à
	 * jour est bien plus lent.
	 * Cette classe implémente IGrid. Elle stocke le nombre de cellules vivantes autour d'une case donnée dans un short,
	 * combiné avec la cellule dans un Tuple.
	 * @brief Grille de cellules appartenant à une seule population.
	 */
	class SinglePopGrid : IGrid {
		private LoopingMatrix<Tuple<Cell, short>> m_cells;

		/**
		 * Ce constructeur remplit de manière aléatoire une matrice de Tuples. Les compteurs sont mis à zéro, mais si
		 * une cellule est crée vivante, un delta est créé. Ils sont résolus après l'initialisation de la grille.
		 */
		public SinglePopGrid(WeightedRandomGenerator weightedRandomGenerator, int width, int height) {
			//Création de la matrice
			m_cells = new LoopingMatrix<Tuple<Cell, short>>(width, height);

			//Liste des deltas à appliquer aux compteurs après la création des cellules
			var deltas = new List<IMatrixModifier>();

			//Création aléatoire des cellules, et stockage du delta des compteurs alentour en cas de cellule vivante
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					//Création de l'état initial de la cellule
					var state = weightedRandomGenerator.GenerateWeightedBool();

					//Création de la cellule
					m_cells.InternalMatrix[i, j] = new Tuple<Cell, short>(new Cell(state), 0);
					//Création du delta si la cellule crée est vivante
					deltas.Add(new CirclingModifier(new OnePopDelta(m_cells, 1), i, j));
				}
			}

			//Pour chaque cellule vivante, incrémentation des compteurs de cellules vivantes tout autour
			foreach (var delta in deltas) {
				delta.ApplyModification();
			}
		}

		/**
		 * Cette méthode met à jour la grille. Elle lit pour chaque case le nombre de cellules vivantes alentour, et
		 * selon ce nombre tente de changer l'état de la cellule concernée. Si la cellule a bien changé d'état, un delta
		 * est crée et la grille est ensuite mise à jour à l'aide de ceux-ci.
		 */
		public void Update() {
			//Liste des deltas résultants de la mort ou de la naissance de cellule lors de la mise à jour
			var deltas = new List<IMatrixModifier>();

			//Mise à jour de chacune des cases de la matrice
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					var tuple = m_cells.InternalMatrix[i, j];

					if (tuple.Item2 < 2 || tuple.Item2 > 3) {
						//La cellule doit mourrir
						if (tuple.Item1.SetState(false)) {
							//Comme la cellule est bien morte et n'est pas juste restée morte, mise à jour de la matrice
							deltas.Add(new CirclingModifier(new OnePopDelta(m_cells, -1), i, j));
						}
					}
					else if (tuple.Item2 == 3) {
						//La cellule doit naître
						if (tuple.Item1.SetState(true)) {
							//Comme la cellule est bien née et n'est pas juste restée vivante, mise à jour de la matrice
							deltas.Add(new CirclingModifier(new OnePopDelta(m_cells, 1), i, j));
						}
					}
				}
			}

			//Application des deltas
			foreach (var delta in deltas) {
				delta.ApplyModification();
			}
		}

		/**
		 * Cette méthode stocke l'état des cellules dans la grille dans un object GridMemento pour pouvoir le comparer
		 * plus tard à un autre GridMemento.
		 * @return Une instance contenant l'état actuel des cellules de la grille.
		 */
		public GridMemento Serialize() {
			return new GridMemento(m_cells);
		}

		/**
		 * Cette méthode affiche la grille. Comme il n'y a qu'une population, deux méthode sont possibles : un affichage
		 * séquentiel qui écrit caractère par caractère, ou une affichage d'une string crée au préalable. La première
		 * méthode est plus rapide, mais la second offre un résultat de bien meilleure qualité, sans clignotement. La
		 * méthode choisie ici est la seconde.
		 */
		public void Draw() {
			//Création de l'objet qui contiendra les caractères à afficher
			var builder = new StringBuilder(m_cells.InternalMatrix.Length);

			//Remplissage du builder avec les caractères des cellules
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					builder.Append(m_cells.InternalMatrix[i, j].Item1.GetDrawnChar());
				}

				builder.Append('\n');
			}

			//Affichage de la string
			Console.WriteLine(builder.ToString());
		}
	}

	/**
	 * Cette classe est une implémentation de IGrid capable de gérer n populations (dans les limites du rasionnable bien
	 * sûr). Elle contient donc une matrice de Tuples, qui contiennent chacun une Cell et un tableau de shorts. Ce
	 * tableau contient le nombre de cellules vivantes des différentes populations autour de la cellule centrale, ce qui
	 * permet de rendre la mise à jour de la grille plus rapide.
	 * @brief Grille de cellules appartenant à plusieurs populations.
	 */
	class MultiPopGrid : IGrid {
		private LoopingMatrix<Tuple<CellPop, short[]>> m_cells;

		/**
		 * Ce constructeur alloue une grille de la taille demandée en argument, tant que ces dimensions sont sensées. De
		 * plus, il remplit cette grille de cellules dont l'état est choisi selon le générateur passée en argument.
		 * Enfin, il prépare la grille à une utilisation rapide, en optimisant les mises à jour futures. Le nombre de
		 * populations est considéré supérieur à zéro.
		 * @param width La largeur de la grille de cellules, qui doit être d'au moins 2.
		 * @param height La hauteur de la grille de cellules, qui doit être d'au moins 2.
		 * @param populationNumber Le nombre de populations qui coexisteront dans la grille.
		 * @param weightedRandomGenerator Le générateur de nombres pseudo-aléatoires utilisé pour le choix des états des
		 * cellules.
		 * @throws ArgumentOutOfRangeException si la taille requise en argument est inférieure à 2 dans une des
		 * dimensions.
		 */
		public MultiPopGrid(WeightedRandomGenerator weightedRandomGenerator, int width, int height,
			int populationNumber) {
			//Création de la matrice virtuelle
			m_cells = new LoopingMatrix<Tuple<CellPop, short[]>>(width, height);

			//Liste des deltas à appliquer aux compteurs après la création des cellules
			var deltas = new List<IMatrixModifier>();

			//Création aléatoire des cellules, et stockage du delta des compteurs alentour en cas de cellule vivante.
			var random = new Random();
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					//Création aléatoire de l'état initial et de la population de la cellule
					var state = weightedRandomGenerator.GenerateWeightedBool();
					var populationId = state ? random.Next(0, populationNumber) : -1;
					//Création de la cellule
					m_cells.InternalMatrix[i, j] =
						new Tuple<CellPop, short[]>(new CellPop(state, populationId), new short[populationNumber]);
					//Création du delta si la cellule crée est vivante
					if (state) {
						deltas.Add(new CirclingModifier(new MultiPopDelta(m_cells, 1, populationId), i, j));
					}
				}
			}

			//Pour chaque cellule vivante, incrémentation des compteurs de cellules vivantes tout autour
			foreach (var delta in deltas) {
				delta.ApplyModification();
			}
		}

		/**
		 * Cette méthode met à jour la matrice de cellules. Elle évalue d'abord le nouvel état de chacune des cellules.
		 * Si le nouvel état d'une cellule est différent de l'état précédent de la cellule, une commande de modification
		 * du nombre de cellules vivantes est générée. Après avoir évalué les états de toutes les cellules, les
		 * commandes sont exécutées. De cette manière, seuls les changements d'états induisent un accès aux cases
		 * alentour.
		 */
		public void Update() {
			//Liste des deltas créés pendant l'exécution de l'algorithme
			var deltaCommands = new List<IMatrixModifier>();

			//Application de la logique à chaque cellule de la matrice
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					//Récupération du tuple en [i, j]
					var tuple = m_cells.InternalMatrix[i, j];

					//Le nombre de cellules de la même population autour de [i, j] est placé dans count
					var cellId = tuple.Item1.GetPopulationId();
					var count = cellId >= 0 ? tuple.Item2[cellId] : 0;

					//Flag permet de savoir si la cellule est passée de vivante à morte ou si elle est restée morte
					var flag = false;

					//Si le compte est à zéro, la cellule meurt (ou reste morte)
					if (count < 2 || count > 3) {
						if (tuple.Item1.SetState(false, -1)) {
							//La cellule a bien changé d'état. Il faut donc mettre à jour les compteurs.
							deltaCommands.Add(new CirclingModifier(new MultiPopDelta(m_cells, -1, cellId), i, j));
							flag = true;
						}
					}

					//Si flag est false, la cellule était soit morte et l'est resté, soit vivante et l'est resté. Si
					//elle était vivante et l'est resté, il n'y a rien de plus à faire. Si elle était morte, on veut
					//voir si elle peut naître.
					if (!flag && cellId == -1) {
						//Liste des populations d'effectif 3 autour de la cellule considérée
						var potentialNewCellIds = new List<int>();
						for (int k = 0; k < tuple.Item2.Length; k++) {
							if (tuple.Item2[k] == 3) potentialNewCellIds.Add(k);
						}

						//Si une seule population a trois cellules vivantes autour de la cellule considérée, la cellule
						//nait de cette population.
						if (potentialNewCellIds.Count == 1) {
							tuple.Item1.SetState(true, potentialNewCellIds[0]);
							deltaCommands.Add(new CirclingModifier(new MultiPopDelta(m_cells, 1,
								potentialNewCellIds[0]), i, j));
						}
						//Si il y a deux populations en compétition, il faut compter un rang plus loin.
						else if (potentialNewCellIds.Count == 2) {
							for (int i1 = 0; i1 < 2; i1++) {
								//On crée des visitors qui comptent le nombre de cellules vivante une première fois à
								//l'ordre deux puis sur toute la grille si nécessaire.
								var counter1 = 0;
								var counter2 = 0;

								if (i1 == 0) {
									//Comptage ordre deux
									for (int k = i - 2; k < i + 3; k++) {
										for (int l = j - 2; l < j + 3; l++) {
											var cell = m_cells.LoopingGet(k, l).Item1;
											if (cell.GetPopulationId() == potentialNewCellIds[0]) counter1++;
											else if (cell.GetPopulationId() == potentialNewCellIds[1]) counter2++;
										}
									}
								}
								else {
									//Comptage dans toute la grille
									foreach (var tuple1 in m_cells.InternalMatrix) {
										if (tuple1.Item1.GetPopulationId() == potentialNewCellIds[0]) counter1++;
										else if (tuple1.Item1.GetPopulationId() == potentialNewCellIds[1]) counter2++;
									}
								}

								//Création de la cellule si non égalité
								if (counter1 > counter2) {
									tuple.Item1.SetState(true, potentialNewCellIds[0]);
									deltaCommands.Add(new CirclingModifier(new MultiPopDelta(m_cells, 1,
										potentialNewCellIds[0]), i, j));
									break;
								}

								if (counter1 < counter2) {
									tuple.Item1.SetState(true, potentialNewCellIds[1]);
									deltaCommands.Add(new CirclingModifier(new MultiPopDelta(m_cells, 1,
										potentialNewCellIds[1]), i, j));
									break;
								}
							}
						}
					}
				}
			}

			foreach (var command in deltaCommands) {
				command.ApplyModification();
			}
		}

		/**
		 * Cette méthode stocke l'état des cellules dans la grille dans un object GridMemento pour pouvoir le comparer
		 * plus tard à un autre GridMemento.
		 * @return Une instance contenant l'état actuel des cellules de la grille.
		 */
		public GridMemento Serialize() {
			return new GridMemento(m_cells);
		}

		/**
		 * Cette méthode affiche la grille avec des caractères en couleurs pour les cellules vivantes et un espace pour
		 * les cellule mortes.
		 */
		public void Draw() {
			var previousColor = ConsoleColor.Black;
			var newColor = previousColor;
			for (int i = 0; i < m_cells.InternalMatrix.GetLength(0); i++) {
				for (int j = 0; j < m_cells.InternalMatrix.GetLength(1); j++) {
					var cell = m_cells.InternalMatrix[i, j].Item1;
					var c = cell.GetDrawnChar();
					if (c == ' ') Console.Write(' ');
					else {
						newColor = cell.GetConsoleColor();
						if (newColor != previousColor) {
							Console.ForegroundColor = newColor;
							previousColor = newColor;
						}

						Console.Write(c);
					}
				}

				Console.WriteLine();
			}
		}
	}

	/**
	 * Cette interface représente un objet capable de créer des IGrid.
	 * @brief Factory de IGrid
	 */
	interface IGridFactory {
		/**
		 * Cette méthode retourne une IGrid au client.
		 * @return Une IGrid correctement crée.
		 */
		IGrid GetGrid();
	}

	/**
	 * Cette classe est capable de construire des grilles de cellules selon les paramètres qui lui sont fournis à la
	 * construction. Elle choisira l'implémentation de IGrid la plus appropriée.
	 * @brief Factory de IGrid
	 */
	class GridFactory : IGridFactory {
		private readonly WeightedRandomGenerator m_generator;
		private readonly Tuple<int, int> m_dimensions;
		private readonly int m_populationNumber;

		/**
		 * Ce constructeur vérifie que les valeurs passées en argument sont valides, et peuvent mener à la création de
		 * grilles fonctionnelles. Il stocke ensuite ces données, pour pouvoir créer des grilles ainsi parametrées à la
		 * volée.
		 */
		public GridFactory(WeightedRandomGenerator generator, Tuple<int, int> dimensions, int populationNumber) {
			m_generator = generator;
			if (dimensions.Item1 < 3 || dimensions.Item2 < 3) throw new ArgumentOutOfRangeException();
			if (dimensions.Item1 > 10000 || dimensions.Item2 > 10000) throw new ArgumentOutOfRangeException();
			m_dimensions = dimensions;
			if (populationNumber < 1 || populationNumber > 10000) throw new ArgumentException();
			m_populationNumber = populationNumber;
		}

		/**
		 * Cette méthode crée une grille avec les paramètres fournis à la construction.
		 * @return Une grille optimisée pour le nombre de populations.
		 */
		public IGrid GetGrid() {
			if (m_populationNumber == 0) return new SinglePopGrid(m_generator, m_dimensions.Item1, m_dimensions.Item2);
			else return new MultiPopGrid(m_generator, m_dimensions.Item1, m_dimensions.Item2, m_populationNumber);
		}
	}

	/**
	 * Cette classe est une implémentation de IGridFactory qui, au lieu de regénérer une grille, fournit à chaque appel
	 * la même grille que celle fournie à la construction.
	 * @brief Factory d'une même IGrid
	 */
	class GridForwarder : IGridFactory {
		private readonly IGrid m_grid;

		/**
		 * Ce constructeur stocke la grille pour la fournir à chaque appel de GetGrid.
		 * @param grid La grille à redonner à chaque fois.
		 */
		public GridForwarder(IGrid grid) {
			m_grid = grid;
		}

		/**
		 * Cette méthode fournit toujours la même grille.
		 * @return La IGrid stockée à la construction.
		 */
		public IGrid GetGrid() {
			return m_grid;
		}
	}

	/**
	 * Cette interface représente une exécution du programme, de telle sorte que l'exécution puisse être automatique ou
	 * dynamique par exemple.
	 * @brief Mode d'exécution du programme
	 */
	interface IRunMode {
		/**
		 * Cette méthode lance le jeu de la vie.
		 * @param grid La grille de cellules avec laquelle exécuter le programme.
		 * @return True si l'exécution s'est terminée de la manière qualifiable de standarde pour l'algorithme, ou false
		 * si elle a été interompue par l'utilisateur de manière impromptue, ou a recontré une erreur.
		 */
		bool Run(IGridFactory gridFactory);

		/**
		 * Planned for later.
		 */
		void Serialize();
	}

	/**
	 * Cette classe représente un mode d'exécution du programme manuel. L'utilisateur a le contrôle de l'avancement des
	 * générations.
	 * @brief Mode d'exécution manuel.
	 */
	class DefaultRunMode : IRunMode {
		/**
		 * Cette méthode lance un jeu de la vie dans lequel l'utilisateur doit cliquer pour faire avancer la génération.
		 * Il peut également effectuer des sauts de générations, ou laisser le programme tourner visuellement.
		 * @param grid La grille de cellules avec laquelle exécuter le programme.
		 * @return Seul l'utilisateur peut décider d'interrompre l'exécution, donc cette méthode retournera toujours
		 * true.
		 */
		public bool Run(IGridFactory gridFactory) {
			//Instanciation d'une grille
			var grid = gridFactory.GetGrid();

			//A value for c which has no impact on the loop
			const char neutralChar = ' ';

			var c = neutralChar;
			var fastForward = false;

			for (long i = 0; c != 'q'; i++) {
				//Affichage de la grille
				Console.Clear();
				grid.Draw();

				//Affichage de la génération
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine("Génération : " + i);

				//Mise à jour de l'entrée de l'utilisateur
				if (fastForward) {
					if (Console.KeyAvailable) {
						c = Console.ReadKey(true).KeyChar;
					}
				}
				else {
					c = Console.ReadKey(true).KeyChar;
				}

				//Changement du mode de déroulement instantané de la boucle
				if (c == 'f') {
					fastForward = !fastForward;
					c = neutralChar;
				}

				//Saut de générations
				if (c == 's') {
					Console.Clear();
					var input = CommunicationUtilities.GetIntInput(
						"Combien de générations voulez-vous sauter?\nChoix " +
						"(0-∞) : ");
					Console.Clear();
					Console.WriteLine("Processing...");
					for (int j = 0; j < input - 1; j++) {
						grid.Update();
					}

					i += input - 1;
					c = neutralChar;
				}

				//Mise à jour de la grille
				grid.Update();
			}

			return true;
		}

		/**
		 * Cette méthode n'est pas implémentée.
		 */
		public void Serialize() {
			throw new NotImplementedException();
		}
	}

	/**
	 * Cette classe représente un mode d'exécution automatique, où l'ordinateur cherche un groupe de cellules de période
	 * trois, pour le montrer à l'utilisateur.
	 * @brief Mode d'exécution automatique.
	 */
	class AutoRunMode : IRunMode {
		public bool Run(IGridFactory gridFactory) {
			//Suit les entrées de l'utilisateur pour savoir si la fonction est annulée ou si elle se termine par un
			//succès.
			var interrupted = true;

			var quit = false;

			//Exécute la boucle tant que l'utilisateur ne souhaite pas quitter l'auto-recherche
			while (!quit) {
				//La grille à tester
				var grid = gridFactory.GetGrid();

				//Suit si la grille est stable et si il faut la regénérer ou non
				var stabilityFound = false;
				//Suit le nombre de mises à jours effectuées, pour que la grille soit regénérée à partir de 140.000
				//mises à jour, pour ne pas bloquer le programme en cas de groupe de cellules se déplaçant (missiles)
				var updateCount = 0;

				var exponent = 1.0;

				//Exécute la boucle en remettant à jour la grille tant qu'une stabilité n'a pas été trouvée, tout en
				//limitant le nombre de mises à jour à 45000.
				while (!stabilityFound && !quit) {
					//Met à jour la grille un nombre de fois augmentant à chaque fois que la grille est déterminée
					//instable.
					Console.WriteLine("Fast-forward de la grille...");
					for (int i = 0; i < (int) (100 * Math.Pow(10, exponent)); i++) {
						grid.Update();
					}

					//Mise à jour du compte des updates de la grille.
					updateCount += (int) (100 * Math.Pow(10, exponent));
					//Si le compte est supérieur à 45000, dernier tentative de trouver des groupes de période trois.
					if (updateCount >= 45000) stabilityFound = true;

					exponent += 0.5;

					//Verifie que l'utilisateur ne désire pas quitter le programme
					if (Console.KeyAvailable) {
						var c = Console.ReadKey(true).KeyChar;
						if (c == 'q') quit = true;
					}

					//Affichage de la grille
					Console.Clear();
					grid.Draw();
					Console.ForegroundColor = ConsoleColor.Black;
					Console.WriteLine("Comparaison des générations...");

					//Prend des snapshots de la grille à un écart de deux générations.
					var memento1 = grid.Serialize();
					grid.Update();
					grid.Update();
					var memento2 = grid.Serialize();

					//Compare les deux mementos
					if (memento1.Equals(memento2)) {
						//Si les mementos sont égaux, on peut partir du principe que la grille est consituée de groupes
						//de cellules qui changent avec une période de 2. Il faut donc régénérer la grille.
						stabilityFound = true;
						Console.WriteLine("La grille est inintéressante. Une nouvelle a été crée.");
					}
					else {
						//Il y a peut-être un groupe de cellules de période supérieure à 2, ou tout simplement la grille
						//est encore instable, auquel cas il faut la remettre à jour. Faire ces quatre mises à jour
						//permet de trouver une périodicité de 3 associée à la périodicité de 2 d'autres groupes.
						grid.Update();
						grid.Update();
						grid.Update();
						grid.Update();
						memento2 = grid.Serialize();
						if (memento1.Equals(memento2)) {
							//Une grille interessante a été trouvée.
							stabilityFound = true;

							//Préviens l'utilisateur et lui donne le contrôle
							Console.Clear();
							Console.WriteLine("Une grille intéressante a été trouvée. Appuyez sur une touche pour " +
							                  "passer en mode manuel.");
							Console.ReadKey(true);

							var runner = new DefaultRunMode();
							runner.Run(new GridForwarder(grid));

							//Demande à l'utilisateur si il veut continuer les simulations aléatoires ou arrêter
							var input = CommunicationUtilities.GetBoolInput("Voulez-vous continuer les simulations " +
							                                                "aléatoires?\nChoix (y/n) : ");
							if (!input) {
								quit = true;
								interrupted = false;
							}
						}
						else Console.WriteLine("La grille semble instable.");
					}
				}
			}

			return !interrupted;
		}

		/**
		 * Cette méthode n'est pas implémentée.
		 */
		public void Serialize() {
			throw new NotImplementedException();
		}
	}

	/**
	 * @brief Classe programme principale.
	 */
	internal class Program {
		/**
		 * Cette méthode crée un IRunMode selon les données fournies par l'utilisateur.
		 * @return Un IRunMode créé selon le mode choisi par l'utilisateur.
		 */
		private static IRunMode CreateRunMode() {
			IRunMode runMode = null;

			while (runMode == null) {
				var input = CommunicationUtilities.GetIntInput("Choix (1/2) : ");
				if (input == 1) {
					runMode = new DefaultRunMode();
					Console.WriteLine("\nCe mode possède quatre contrôles.\n\nq permet de quitter la simulation.\ns " +
					                  "permet de sauter un nombre de générations que l'on vous demande.\nf permet " +
					                  "d'activer ou de désactiver le mode fast-forward, qui exécute le programme " +
					                  "sans arrêt. Il est recommandé de le désactiver toutes les 5~10 seconde.\n" +
					                  "Enfin, un appui sur toute autre touche fait avancer la génération de 1 cran.\n");
				}
				else if (input == 2) {
					runMode = new AutoRunMode();
					Console.WriteLine("\nCe mode cherche les groupes de cellules de période trois. Il tourne seul, " +
					                  "sans arrêt tant qu'il n'a rien trouvé.\nIl peut être interrompu en appuyant " +
					                  "sur q. L'arrêt est pris en compte après les mises à jours qui tournent au " +
					                  "moment de l'appui.\n");
				}
				else Console.WriteLine("Veuillez entrer 1 ou 2.");
			}

			//Attente de l'utilisateur, qui quitte les informations suplémentaires sur le choix effectué
			Console.WriteLine("Appuyez sur une touche pour continuer.");
			Console.ReadKey(true);

			return runMode;
		}

		/**
		 * Cette méthode crée un générateur aléatoire de booléens selon le ratio demandé au client.
		 * @return Un WeightedRandomGenerator créé selon le ratio fourni par l'utilisateur.
		 */
		private static WeightedRandomGenerator CreateRandomGenerator() {
			WeightedRandomGenerator gen = null;

			while (gen == null) {
				try {
					//Ratio de cellules vivantes
					var ratio = CommunicationUtilities.GetFloatInput("Quel pourcentage de remplissage voulez-vous?\n" +
					                                                 "(mettez un . à la place des , si vous etes sous Linux)\n" +
					                                                 "Choix (0,1-0,9) : ");

					//Création du générateur
					gen = new WeightedRandomGenerator((float) ratio);
				}
				catch (ArgumentOutOfRangeException) {
					Console.WriteLine("Ce ratio n'est pas compris entre 0.1 et 0.9. Veuillez fournir une valeur " +
					                  "dans cet intervalle.");
				}
			}

			return gen;
		}

		/**
		 * Cette méthode crée une GridFactory de grilles selon les dimensions et le nombre de populations demandées par
		 * le client.
		 * @return Une GridFactory créée selon les données fournies par l'utilisateur.
		 */
		private static GridFactory CreateGridFactory(WeightedRandomGenerator gen) {
			GridFactory gridFactory = null;

			while (gridFactory == null) {
				try {
					//Taille de la grille
					var width = CommunicationUtilities.GetIntInput("Quelle largeur de grille voulez-vous?\nChoix " +
					                                               "(3-10000) : ");
					var height = CommunicationUtilities.GetIntInput("Quelle hauteur de grille voulez-vous?\nChoix " +
					                                                "(3-10000) : ");
					//Taille de la population
					var populationNumber =
						CommunicationUtilities.GetIntInput(
							"Quel nombre de populations voulez-vous? Notez que seules quatorze " +
							"générations peuvent être visualisées avec des couleurs distinctes des cellules " +
							"mortes. Au-delà de quatorze, les couleurs bouclent.\nChoix (1-10000) : ");

					//Création de la grille
					gridFactory = new GridFactory(gen, new Tuple<int, int>(width, height), populationNumber);
				}
				catch (ArgumentOutOfRangeException) {
					Console.WriteLine("Ces tailles ne sont pas valides. Veuillez donner des valeurs plus " +
					                  "grandes que 2, et inférieures à 10000");
				}
				catch (ArgumentException) {
					Console.WriteLine("Le nombre de populations doit être positif. De plus, le nombre de populations " +
					                  "est limité à la quantité symbolique de 10000.");
				}
			}

			return gridFactory;
		}

		/**
		 * Méthode Main. Affiche un message d'accueil, crée les objets nécessaires à l'exécution, puis lance le jeu.
		 */
		public static void Main() {
			//Choix du mode d'utilisation
			Console.Write("Bienvenue!\n\nSouhaitez-vous utiliser le programme en mode manuel (1) ou en mode " +
			              "automatique (2)?\n\nLe mode manuel permet de passer les générations une par une, de faire " +
			              "des sauts en avant ou de faire aller le programme à vitesse maximale.\nLe mode " +
			              "automatique recherche seul des groupes de cellules de période trois ou quatre, puis donne " +
			              "le contrôle sur la grille à l'utilisateur, comme en mode 1.\n");

			//Mode d'exécution choisi par l'utilisateur
			var runMode = CreateRunMode();
			Console.Clear();

			//Générateur de bool aléatoire
			var gen = CreateRandomGenerator();
			Console.WriteLine();
			//Factory de grilles
			var gridFactory = CreateGridFactory(gen);

			//Exécution du jeu de la vie dans le mode choisi
			runMode.Run(gridFactory);
		}
	}
}