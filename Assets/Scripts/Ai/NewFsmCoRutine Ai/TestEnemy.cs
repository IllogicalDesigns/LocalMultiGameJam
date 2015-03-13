using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class TestEnemy : MonoBehaviour
{
		public LayerMask m_TargetMask;
		public float m_SightRange = 5.5f;
		public int m_CurrentHealth = 100;
		public enum m_AiStates
		{
				patrol,				//search area to look for potential problems
				Combat,				//decide if it is possible to eliminate threats
				Searching,			//Something is fishy seek to destroy any threats	
				Survival			//my safety is more important then my job "high stress"
		}
		public float m_Health = 1.0f, m_Stress = 0;
		private SphereCollider m_sphereCol;
		private NavMeshAgent m_Agent;

		void Start ()
		{
				m_sphereCol = gameObject.GetComponent<SphereCollider> ();
				m_Agent = gameObject.GetComponent<NavMeshAgent> ();
		}
		//when someone enters our stress area
		void OnTriggerEnter (Collider col)
		{
				if (col.tag == "Player" || col.tag == "Enemy") {
						StressCalculation ();
						if (m_Stress > 0 && col.tag == "Player" && Vector3.Dot ((col.transform.position - transform.position).normalized, transform.forward) > 0.75f)
								m_Agent.SetDestination (col.transform.position);
				}

		}

		void OnTriggerExit (Collider col)
		{
				if (col.tag == "Player" || col.tag == "Enemy") {
						StressCalculation ();
						if (m_Stress > 0 && col.tag == "Player" && Vector3.Dot ((col.transform.position - transform.position).normalized, transform.forward) > 0.75f)
								m_Agent.SetDestination (col.transform.position);
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
						if (m_col.tag == "Player" && Vector3.Dot ((m_col.transform.position - transform.position).normalized, transform.forward) > 0.75f) {
								TestPlayer m_testPlay = m_col.gameObject.GetComponent<TestPlayer> ();
								m_Stress = m_Stress - m_testPlay.playerIntimidationValue;								//figure out the players worth to scare our Ai
								Debug.DrawLine (transform.position, m_col.transform.position, Color.red, 1f);
								//eventually have them clump up to protect each other
						}
						if (m_col.tag == "Enemy") {
								m_Stress = m_Stress + 10f;
								Debug.DrawLine (transform.position, m_col.transform.position, Color.green, 1f);
						}
				}
		}
		// Update is called once per frame
		void Update ()
		{

		}
}
