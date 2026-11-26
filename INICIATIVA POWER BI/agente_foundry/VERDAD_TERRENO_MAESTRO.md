# Verdad de terreno · caso «maestro» (ejecutado en Workbench el 26-nov-2026)

Fuente: `sql/09_maestro_estudiantes.sql`, base real `gs_data_analysis` (+ `gs_teachers_web_db_prod` para grupos). Solo conteos; sin ids de estudiantes.
Definiciones: conexión = fila de `connected_date` (usuario-día) · «sin conexión reciente» = ninguna en los últimos 30 días de datos · datos de conexión hasta ≈ 25-nov-2026 · prueba completada = `quiz_taken_date` no nulo.

## Vínculo maestro → estudiante (validado)
`teacher_groups.teacher_id` y `teacher_group_students.student_id` = `gs_data_analysis.user.user_id`: 55 de 56 maestros son rol 5 y 314 de 314 estudiantes son rol 3.

## Resumen por maestro (sección 5 del script)
| teacher_id | Estudiantes | Nunca conectados | Sin conexión reciente (30 d) | Pruebas asignadas | Completadas |
|---|---|---|---|---|---|
| 10183 | 20 | 10 | 10 (último acceso 20-nov-2025) | 0 | 0 |
| 1312 | 18 | 0 | 18 (últimos accesos ene-2025 … ene-2026) | 0 | 0 |
| 1336 | 137 | 0 | 136 | 627 | 16 (2,55 %) |

- 1336: 24 de sus 137 estudiantes tienen pruebas asignadas (627 asignadas, 16 completadas), verificado con consulta independiente.
- Solo 10183 y 1312 están fuera del ORE DEMOSTRACION; 1336 es el único con pruebas para validar progreso y puntuaciones (cuenta de prueba/demo).

## Cuidados
- Un estudiante puede estar en varios grupos del mismo maestro: contar siempre por estudiante distinto (si no, se sobrecuentan las pruebas: 711/31 en vez de 627/16).
- Sin cuentas de prueba, no hay pruebas reales recientes (ver `PLAN_PRUEBAS_REALES_Y_PANEL.md`): el agente debe decirlo, no inventar progreso ni puntuaciones.
