{{- define "taskly.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- define "taskly.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- $name := default .Chart.Name .Values.nameOverride -}}
{{- if contains $name .Release.Name -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}
{{- end }}

{{- define "taskly.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- define "taskly.labels" -}}
helm.sh/chart: {{ include "taskly.chart" . }}
app.kubernetes.io/name: {{ include "taskly.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}




{{- define "taskly.common.selectorBase" -}}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/part-of: taskly
{{- end -}}



{{- define "taskly.backend.selectorLabels" -}}
{{ include "taskly.common.selectorBase" . }}
app.kubernetes.io/name: backend
app.kubernetes.io/component: backend
app.kubernetes.io/version: {{ .Chart.AppVersion | default .Chart.Version }}
{{- end -}}

{{- define "taskly.backend.labels" -}}
{{ include "taskly.backend.selectorLabels" . }}
{{- end -}}

{{- define "taskly.backend.fullname" -}}
{{ include "taskly.fullname" . }}-backend
{{- end -}}



{{- define "taskly.frontend.selectorLabels" -}}
{{ include "taskly.common.selectorBase" . }}
app.kubernetes.io/name: frontend
app.kubernetes.io/component: frontend
app.kubernetes.io/version: {{ .Chart.AppVersion | default .Chart.Version }}
{{- end -}}

{{- define "taskly.frontend.labels" -}}
{{ include "taskly.frontend.selectorLabels" . }}
{{- end -}}

{{- define "taskly.frontend.fullname" -}}
{{ include "taskly.fullname" . }}-frontend
{{- end -}}



{{- define "taskly.SecretName" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end -}}
