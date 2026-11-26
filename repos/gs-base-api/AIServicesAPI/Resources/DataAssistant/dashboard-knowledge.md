# Knowledge base: Power BI dashboard "MSJ_Utilization" (Estudia y Avanza)

Use this text ONLY to explain what the dashboard shows and how it calculates. It contains no data: every figure must come from a query. Page, measure and term names are kept in Spanish, exactly as they appear in the dashboard.

## 1. What the dashboard is

- It shows the use of the Estudia y Avanza platform in the schools of San Juan: users, connections, enrollments (subscriptions), school visits, teacher activity and test results.
- Data comes from the platform database and from an Excel file, maintained by hand, with the schools that were visited. It is not real time: it reaches the last data load, and how often it refreshes is not documented.
- A CONNECTION is one person on one day. The dashboard has no sessions, no time on the platform and no hours: those questions cannot be answered.
- An ACTIVE USER is a user with at least one subscription (matrícula) in status active. The dashboard glossary calls it "Matrícula Activa".
- A CANCELED user has at least one canceled subscription. A user can be active and canceled at the same time (several subscriptions), so active, canceled and without subscription can add up to more than the total users.
- A user "sin subscripción" has no subscription registered.
- Roles in the data: student, teacher, director and admin-ore. The role client and the school "Escuela Facilitadores MSJ" are left out of the user counts; some demo schools are left out of the teacher, course and enrollment history pages.
- School systems: SEMI (schools of the Sistema Educativo Municipio de San Juan Integrado), Alianza (schools with an alliance with SEMI) and ORE (schools of the ORE de San Juan, Departamento de Educación). The dashboard groups SEMI and Alianza in one value, "SEMI/Alianza".
- The dashboard is the Power BI report "MSJ_Utilization" (workspace MSJ). The platform shows its pages embedded in the reports menu, under "Vista administrativa" ("Reporte de conexión", "Reporte acumulativo de conexiones", "Reporte de uso por maestros", matching by name the pages Usuarios_Conexiones (administrative) and % de Conexiones redesign (administrativo demo), Conexiones Acumulativo (administrative) and Teachers_Activity_(administrative)). The Dashboard context sent with a question names those platform menu entries, not the Power BI report: when asked for the name of the dashboard, say MSJ_Utilization. The exact correspondence between menu entries and pages is not documented.
- Some slicers (filters) are saved with a selection (for example a range of dates in "Fecha de Conexión"), so the numbers shown on opening may be limited to that selection.

## 2. Pages (13 visible, the rest hidden)

The page that opens by default is "% de Conexiones redesign (administrativo demo)". The audience is only a hint from the page name: "administrative" / "administrativo" = administrators; "end user-demo" = end users (the dashboard does not say if director, ORE or teacher). Several pages are marked "demo", "redesign" or "opción 1/2": the dashboard does not say which is the definitive one.

- **% de Conexiones redesign (administrativo demo)**: percentage of active users that connected, by role and by school, with the summary of visited schools. Shows connected vs not connected users by role and by school, a list of connected users, a list of users not connected, the cards "Escuelas" (total, selected, visited, not visited) and "Conexiones" (connected schools, unique connected users, connections, active users and "% Conexión Global").
- **Usuarios_Conexiones (administrative)**: how many users connected, how many did not and how many connections there are, by school and role. Cards: total of users with active enrollment, total of connections, "% Conexión", connected users, connected schools; donut "Usuarios Conectados vs No Conectados"; bars "Conexiones por Escuela y por Rol"; lists of connected and not connected users.
- **Conexiones Acumulativo (administrative)**: how connections evolve by date and the difference with the previous day ("Conexiones Por Fecha", "Diferencia en Conectados por Fecha").
- **Conexiones Acumulativo - redesign (administrative)**: redesigned version: unique users, connections and daily variation, by role, school and date.
- **Teachers_Activity_(administrative)**: whether teachers created instructional plans, groups and assignments, by school and period ("Acciones Tomadas por el Maestro"). The page note says CRECE reviews are considered, so a teacher can have actions without connections.
- **Escuelas Visitadas**: which schools were visited and which were not (from the Excel file).
- **Reporte de Usuarios (end user-demo)** and **Reporte de Usuarios - opción 2 (end user-demo)**: total schools and users, by role and by school, and a list of users; the second also shows schools by education system and active teachers and students (based on active courses).
- **Reporte Académico (end user-demo)**: test results by school, grade and subject, tests assigned and completed, students who took a test and "% Estatus".
- **Historial de matrícula por grado (opción 1)** and **(opción 2)**: users and schools by academic year, plan (grade), school, role and enrollment status.
- **Glosario**: the dashboard's own definitions of terms.
- **Page 1** (visible): table with activity indicators by teacher and school; looks like an internal review page.
- Hidden pages (not meant for users): "Teachers_Activity" (old version), "Reporte de Conexiones (end user-demo)" (marked "UNDER REVIEW"), "Historial de matrícula (fecha de activación)", "Duplicate of Usuarios_Conexiones (administrative)", "Duplicate of Page 1", "check_list", "checklist usuarios activos", "Page 2", "Page 3" and "Page 4" (internal review tables).

## 3. Terms and how they are calculated

**Connections and users**
- **Conexiones / Total de conexiones** (Conteo_Conectados_Si_Total): number of connection rows; one row = one person on one day. Several logins on the same day count once. Measures frequency of use, not unique adoption.
- **Usuarios Conectados / Conectado único**: distinct people (by email) with at least one connection in the chosen dates. A person connected on two days counts once.
- **Usuarios Activos / Total Usuarios Activos (por Rol)**: people with at least one subscription in status active. The latter counts emails, ignores the date, the "Escuela" slicer and "Visitada", and does respond to role, education system and school.
- **Usuarios NO Conectados (Activos por Rol)**: active users minus connected users, never below zero. It is lower than the real number because the connected people include some who are not active.
- **Usuarios no Conectados** (the list/table): comes from a separate list calculated in the database; it does not follow the date, role or education-system slicers, so it can differ from the donut or the measure above.
- **Escuelas Conectadas**: schools with at least one connected user.
- **Última conexión**: most recent connection date. In the lists of connected users it is per person; in "Datos de usuarios" it is the most recent of the whole school, because connections are linked to users by school and not by person.
- **Diferencia Conectados** (variation vs previous day): connections of the day minus connections of the previous calendar day. It compares connections, not people, and it is NOT a comparison between two periods: it only compares each day with the previous day (a comparison of two periods, such as September against October, is not a measure of the dashboard). It is empty when the previous day had no connections and negative when there were fewer. When a month or year is chosen it compares the last date of the range with its previous day.
- **Estatus al momento de la conexión** (subscription_status): the subscription state of the person when the connection was recorded: active, inactive or "sin matrícula".

**Percentages of connection (there is no single formula)**
- General rule: connected users divided by active users. The numerator counts everyone who connected in the chosen dates, with or without an active subscription today, while the divisor counts only active users. For that reason a percentage CAN PASS 100 %; it is not an error (it happens mostly in schools with few active users and many connected ones).
- **% Conexión** (page Usuarios_Conexiones): distinct connected users divided by the total of users with active enrollment. That total is fixed: it does not change with slicers, so choosing one school gives a very low percentage.
- **% Conexión Global** (page % de Conexiones redesign): unique connected users divided by active users (Total Usuarios Activos (por Rol)).
- **% Usuarios Conectados**: Usuarios Conectados (Activos por Rol) divided by Total Usuarios Activos (por Rol); a proportion (0.35 = 35 %).
- **% usuarios conectados por escuela**: connected users of the school per 100 active users of that school.
- **% Conexión por Rol**: students, teachers or directors connected divided by the active ones of that role.
- Despite its name, "Usuarios Conectados (Activos por Rol)" does not filter by active status.
- In the donut "Usuarios Conectados vs No Conectados" the slicers "Escuela", "Rol", "Visitada" and "Fecha de Conexión" do not reduce the total of active users, so for the value of ONE school use the page % de Conexiones redesign.

**Enrollments (subscriptions)**
- A subscription belongs to one school and one plan and groups several users; a user can have several subscriptions. Its dates: activation (activated_date), attached to the enrollment (user_attached_date, "Adjuntado a matrícula") and cancellation (cancellation_date, "Fecha de cancelación"; in active subscriptions it is a future date, probably the scheduled end).
- **Plan**: the product of the subscription; the plans with active users are grades from Kindergarten to Duodécimo.
- **Academic year**: depends on the table. In tests, from August to July. In subscriptions, from the year of the end date (an active subscription that ends in 2027 is "2026-2027"). In connections, the calendar year shifted without looking at the month.
- **Total Usuarios**: distinct users with any subscription status; it is not the same as active users.

**Schools**
- **Visitada (Si/No)**, **Visitadas_Si**, **Visitadas_No**: schools visited / not visited, according to the manual Excel file. "Visitada" means the school received a visit, training or support; the dashboard does not record which type.
- **Total escuelas fijo**: total of schools, ignoring every filter.
- **Sistema Educativo**: SEMI/Alianza or ORE (see section 1).

**Teachers**
- **Acciones de maestros**: five Yes/No indicators calculated in the database: had a connection, created instructional plans, plans with content/data, created groups (in the year) and created assignments. The chart shows the last four; the connection one is used as a slicer.
- **Maestros con Actividad**: counts all the teachers listed in the report; it does not verify that they did any action, although the glossary says "who performed actions".
- **Conteo_SI / Conteo_NO**: number of "Si" / "No" answers of the indicators; they count rows (teacher, period, indicator), not teachers, so a teacher with several periods counts several times.
- **Período**: semester of an activity ("1er Sem" Jul-Dec, "2do Sem" Jan-Jun); empty when the period crosses from one year to the next. The dates of each teacher period come from the database and can include future periods.

**Tests (Reporte Académico)**
- **Quizz_taken / Completadas**: tests completed by the student. **Quizz_not_taken / No completadas**: tests assigned and not completed; their score is 0 by default. **Quizz_assigned / Asignadas** = completed + not completed (counted as student-test rows).
- **Puntuación (%) / % Full Quiz (promedio)**: average of the full-quiz score, from 0 to 100. Not completed tests have score 0 and lower the average if they are not filtered out; the report page uses only completed tests and leaves out "Posprueba". The glossary says "only tests that count for the grade", but the data has no column that tells it.
- **Period (P1 to P4)**: academic period of a test.

## 4. How to answer

- "What is this page / this chart?": use the page name in the Dashboard context, if any, and the pages above.
- Two figures that differ: explain the likely definitional reason from this text (different divisor, fixed total, list vs measure, connections vs people, slicers that do not filter a card) and do not say which one is "correct" unless the text says so.
- Not defined in the dashboard (say it is not documented and suggest asking the dashboard owner): which page is the definitive one, exact audience of each page, what exactly a connection registers, how some totals and teacher indicators are calculated inside the database, how subscriptions are assigned to users, refresh schedule, why "Posprueba" is excluded and why some teachers are excluded by name.
- Row-level data (a person's name, email or grades) is not part of this knowledge: do not reproduce it.
