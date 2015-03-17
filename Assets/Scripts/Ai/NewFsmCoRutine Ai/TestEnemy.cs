using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class TestEnemy : MonoBehaviour
{
		public enum m_AiStates
		{
				patrol,				//search area to look for potential problems
				Combat,				//decide if it is possible to eliminate threats
				Searching,			//Something is fishy seek to destroy any threats	
				Survival			//my safety is more important then my job "high stress"
		}
		public LayerMask m_TargetMask;
		public m_AiStates myState;
		public float m_SightRange = 5.5f;
		public int m_CurrentHealth = 100;
		public float visionAngle = 0.75f;		//0.75f is a good range
		public float m_Health = 1.0f, m_Stress = 0;
		private SphereCollider m_sphereCol;
		private NavMeshAgent m_Agent;
		public bool m_DebugMode = true;

		void Start ()
		{
				myState = m_AiStates.patrol;
				m_sphereCol = gameObject.GetComponent<SphereCollider> ();
				m_sphereCol.radius = m_SightRange;
				m_Agent = gameObject.GetComponent<NavMeshAgent> ();
		}
		//when someone enters our stress area
		void OnTriggerEnter (Collider col)
		{
				if (col.tag == "Player" || col.tag == "Enemy") {
						StressCalculation ();
						if (m_Stress > -100000 && col.tag == "Player" && Vector3.Dot ((col.transform.position - transform.position).normalized, transform.forward) > visionAngle) {
								m_Agent.SetDestination (col.transform.position);
								myState = m_AiStates.Combat;
								
						}
				}

		}

		void OnTriggerStay (Collider col)
		{
				if (m_Stress > -100000 && col.tag == "Player" && Vector3.Dot ((col.transform.position - transform.position).normalized, transform.forward) > visionAngle) {
						m_Agent.SetDestination (col.transform.position);
						myState = m_AiStates.Combat;
				}
		}

		void OnTriggerExit (Collider col)
		{
				if (col.tag == "Player" || col.tag == "Enemy") {
						StressCalculation ();
						if (m_Stress > -100000 && col.tag == "Player" && Vector3.Dot ((col.transform.position - transform.position).normalized, transform.forward) > visionAngle) {
								m_Agent.SetDestination (col.transform.position);
								myState = m_AiStates.Combat;
						}
				}
		}
		//Used to calculate how stressed out we currently our
		void StressCalculation ()
		{
				m_Stress = -10f;
				m_Stress = m_Stress + m_CurrentHealth;
				if (Input.GetKey (KeyCode.Space)) {
						Debug.Log (m_Stress);
				}
				Collider[] actorsNearMe = Physics.OverlapSphere (transform.position, m_sphereCol.radius, m_TargetMask.value);
				foreach (Collider m_col in actorsNearMe) {
						if (m_col.tag == "Player" && Vector3.Dot ((m_col.transform.position - transform.position).normalized, transform.forward) > visionAngle) {
								TestPlayer m_testPlay = m_col.gameObject.GetComponent<TestPlayer> ();
								m_Stress = m_Stress - m_testPlay.playerIntimidationValue;								//figure out the players worth to scare our Ai
								if (m_DebugMode)
										Debug.DrawLine (transform.position, m_col.transform.position, Color.red, 1f);
								//eventually have them clump up to protect each other
						}
						if (m_col.tag == "Enemy") {
								m_Stress = m_Stress + 10f;
								if (m_DebugMode)
										Debug.DrawLine (transform.position, m_col.transform.position, Color.green, 1f);
						}
				}
		}
		// Update is called once per frame
		void Update ()
		{

		}
}
